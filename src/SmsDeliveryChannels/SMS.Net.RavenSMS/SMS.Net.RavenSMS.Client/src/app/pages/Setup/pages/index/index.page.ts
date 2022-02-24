import { SettingsStoreActions, SettingsStoreSelectors } from 'src/app/store/settings-store';
import { BarcodeScanner, SupportedFormat } from '@capacitor-community/barcode-scanner';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { IQrContentModel } from 'src/app/core/models';
import { AlertController } from '@ionic/angular';
import { RootStoreState } from 'src/app/store';
import { Capacitor } from '@capacitor/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'page-setup-index',
  templateUrl: 'index.page.html',
  styleUrls: ['index.page.scss'],
})
export class IndexPage implements OnInit, OnDestroy {

  subsink = new SubSink();

  message: string;
  scanActive = false;
  permissionGranted = false;
  configurationAllowed = false;
  showOpenSettingsButton = false;
  showGrantPermissionButton = false;

  platform: string;
  configurationForm: FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private alert: AlertController,
    private store: Store<RootStoreState.State>,
  ) { }

  ngOnInit(): void {
    this.subsink.sink = this.store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(async state => {
        if (state.serverInfo?.serverUrl && state.appIdentification?.clientId) {
          console.log('client app already configured, redirecting to the home page...');
          this.router.navigateByUrl('/');
          return;
        }

        this.configurationAllowed = true;
        await this.initialize();
      });
  }

  ngOnDestroy(): void {
    this.subsink.unsubscribe();

    if (this.platform != 'web') {
      BarcodeScanner.stopScan();
    }
  }

  async initialize(): Promise<void> {
    // get the platform
    this.platform = Capacitor.getPlatform();

    // this code is only relevant if we are not previewing in a web browser
    if (this.platform != 'web') {
      // first check for the user permission
      this.message = 'checking camera permission ...';
      this.permissionGranted = await this.checkUserPermission();

      if (this.permissionGranted) {
        this.message = "ready for scan";
      }

      // prepare the barcode scanner
      BarcodeScanner.prepare();
      return;
    }

    this.initializeForm();
  }

  async startScanning(): Promise<void> {
    this.scanActive = true;
    this.message = 'starting the QR scanning ...';

    // start the scanning
    const scanResult = await BarcodeScanner.startScan({
      targetedFormats: [
        SupportedFormat.QR_CODE
      ],
    });

    // set the scanning state to false
    this.scanActive = false;

    // if the result has content
    if (scanResult.hasContent) {
      // read the json content from the QR content
      const model = JSON.parse(atob(scanResult.content)) as IQrContentModel;

      // check if we have any valid content
      if (!model.serverUrl || !model.clientId) {
        this.message = "invalid QR code value, please scan the QR code on the Client Setup page.";
        return;
      }

      // dispatch the configuration action
      this.store.dispatch(SettingsStoreActions.ConfigureClient({ data: model }));

      this.message = 'done...';
      return;
    }

    this.message = "failed to read the QR code, please try again.";
  }

  async checkUserPermission(): Promise<boolean> {
    // check if user already granted permission
    const status = await BarcodeScanner.checkPermission({ force: false });

    // user granted permission
    if (status.granted) {
      return true;
    }

    // user denied permission
    if (status.denied) {
      this.message = "you have denied the camera permission, please enable the permission form the setting.";
      this.showOpenSettingsButton = true;
      return false;
    }

    // user has not been requested this permission before
    if (status.neverAsked) {
      const alertResult = await this.alert.create({
        header: "Permission request",
        message: "allow camera permission to enable the QR code scanner",
        buttons: [
          {
            text: 'No',
            role: 'cancel',
          },
          {
            text: 'Ok',
            role: 'ok',
          }
        ]
      });

      // show the alert
      await alertResult.present();

      // get the result on dismiss
      const dismissResult = await alertResult.onDidDismiss();
      if (dismissResult.role == "cancel") {
        this.message = "you have canceled the request, the camera permission is required to scan the QR";
        this.showGrantPermissionButton = true;
        return false;
      }
    }

    // probably means the permission has been denied (ios only)
    if (status.restricted || status.unknown) {
      return false;
    }

    // user has not denied permission but the user also has not yet granted the permission
    const statusRequest = await BarcodeScanner.checkPermission({ force: true });
    if (statusRequest.asked) {
      // system requested the user for permission during this call
      // only possible when force set to true
    }

    if (statusRequest.granted) {
      // the user did grant the permission now
      return true;
    }

    // user did not grant the permission, so he must have declined the request
    return false;
  }

  grantPermission() {
    BarcodeScanner.checkPermission({ force: true });
  }

  openSettingPage() {
    BarcodeScanner.openAppSettings();
  }

  initializeForm(): void {
    this.configurationForm = this.fb.group({
      clientId: this.fb.control('', [
        Validators.required,
        Validators.maxLength(17),
        Validators.minLength(17),
      ]),
      serverUrl: this.fb.control('', [
        Validators.required,
        Validators.pattern(/^https?:\/\/\w+(\.\w+)*(:[0-9]+)?(\/.*)?$/)
      ]),
      clientName: this.fb.control(''),
      clientDescription: this.fb.control(''),
    });
  }

  submit(): void {
    if (!this.configurationForm.valid) {
      this.message = "invalid form value, please enter a valid values.";
      return;
    }

    var model = this.configurationForm.value as IQrContentModel;

    // check if we have any valid content
    if (!model.serverUrl || !model.clientId) {
      this.message = "invalid form value, please enter a valid values.";
      return;
    }

    // dispatch the configuration action
    this.store.dispatch(SettingsStoreActions.ConfigureClient({ data: model }));
  }
}
