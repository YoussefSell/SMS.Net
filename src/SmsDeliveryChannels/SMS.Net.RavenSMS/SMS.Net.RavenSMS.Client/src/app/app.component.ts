import { RootActions, RootStoreSelectors, RootStoreState, StorePersistenceActions, UIStoreSelectors } from './store';
import { DeviceNetworkStatus, ServerStatus } from './core/constants/enums';
import { SettingsStoreSelectors } from './store/settings-store';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService, SmsService } from './core/services';
import { ToastController } from '@ionic/angular';
import { Network } from '@capacitor/network';
import { IMessages } from './core/models';
import { Router } from '@angular/router';
import { App } from '@capacitor/app';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-root',
  template: `
  <ion-app [class.dark-theme]="dark">
    <ion-router-outlet id="main-content"></ion-router-outlet>
  </ion-app>
  `,
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {

  _subSink = new SubSink();
  dark: boolean = false;

  constructor(
    private _router: Router,
    private _smsService: SmsService,
    private _signalRService: SignalRService,
    private _toastController: ToastController,
    private _store: Store<RootStoreState.State>,
  ) {
    // add a listener on the app state
    App.addListener('appStateChange', ({ isActive }) => {
      if (!isActive) {
        this._store.dispatch(StorePersistenceActions.PersistStore());
      }
    });
  }

  async ngOnInit(): Promise<void> {
    Network.addListener('networkStatusChange', status => {
      this._store.dispatch(RootActions.UpdateNetworkConnectionStatus({
        newStatus: status.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
      }));
    });

    this._subSink.sink = this._store.select(UIStoreSelectors.IsDarkModeSelector)
      .subscribe(value => this.dark = value);

    this._subSink.sink = this._store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        if (state.serverInfo?.serverUrl && state.appIdentification?.clientId) {
          this._signalRService.initConnection(state.serverInfo?.serverUrl, state.appIdentification?.clientId)
            .then(() => {
              this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.ONLINE }))
            })
            .catch(async (error) => {
              await this.presentToast('connection  failed' + error + state.serverInfo.serverUrl);
              this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE }));
            });
          return;
        }

        // here the app is not configured yet, redirect to the setup page
        this.redirectToSetupPage();
      });

    this._subSink.sink = this._store.select(RootStoreSelectors.ServerConnectionSelector)
      .subscribe(async status => {
        if (status == ServerStatus.ONLINE) {
          await this.presentToast("you have been connected to the server successfully", 3000);
          return;
        }

        //await this.presentToast("failed to connect to server, make sure the server is up, and try again");
      });

    this._subSink.sink = this._store.select(RootStoreSelectors.NetworkConnectionSelector)
      .subscribe(async status => {
        if (status == DeviceNetworkStatus.OFFLINE) {
          await this.presentToast("you have been disconnected, please check your internet connection.");
          return;
        }

        //await this.presentToast("failed to connect to server, make sure the server is up, and try again");
      });

    // register the server events handlers
    this.registerServerEvents();

    // check current network status
    await this.checkCurrentNetworkStatus();
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
    Network.removeAllListeners();
  }

  registerServerEvents(): void {
    // register the handler for the send message event
    this._signalRService.onSendMessageEvent((message: IMessages) => {
      this._smsService.sendSms$(message.to, message.content);
    });
  }

  private async checkCurrentNetworkStatus() {
    const currentNetworkStatus = await Network.getStatus();
    this._store.dispatch(RootActions.UpdateNetworkConnectionStatus({
      newStatus: currentNetworkStatus.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
    }));
  }

  redirectToSetupPage(): void {
    this._router.navigateByUrl('/setup');
  }

  async presentToast(message: string, duration: number | undefined = undefined) {
    const toast = await this._toastController.create({
      duration: duration, message: message,
    });
    toast.present();
  }
}
