import { BarcodeScanner, SupportedFormat } from '@capacitor-community/barcode-scanner';
import { SettingsStoreActions } from 'src/app/store/settings-store';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { IQrContentModel } from 'src/app/core/models';
import { AlertController } from '@ionic/angular';
import { RootStoreState } from 'src/app/store';
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
  showOpenSettingsButton = false;
  showGrantPermissionButton = false;

  constructor(
    private alert: AlertController,
    private store: Store<RootStoreState.State>,
  ) { }

  async ngOnInit(): Promise<void> {
    this.message = 'checking camera permission ...';

    // first check for the user permission
    this.permissionGranted = await this.checkUserPermission();

    if (this.permissionGranted) {
      this.message = "ready for scanning";
    }

    // prepare the barcode scanner
    BarcodeScanner.prepare();
  }

  ngOnDestroy(): void {
    this.subsink.unsubscribe();
    BarcodeScanner.stopScan();
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
    const status = await BarcodeScanner.checkPermission({ force: true });

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
}
