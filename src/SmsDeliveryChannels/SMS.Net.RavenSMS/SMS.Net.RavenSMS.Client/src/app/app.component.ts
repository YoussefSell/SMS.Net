import { MessagesStoreActions, RootActions, RootStoreSelectors, RootStoreState, StorePersistenceActions, UIStoreSelectors } from './store';
import { DeviceNetworkStatus, ServerStatus } from './core/constants/enums';
import { AlertController, ToastController } from '@ionic/angular';
import { SettingsStoreActions, SettingsStoreSelectors } from './store/settings-store';
import { IAppIdentification, IMessages } from './core/models';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService, SmsService } from './core/services';
import { State } from './store/settings-store/state';
import { TranslocoService } from '@ngneat/transloco';
import { Network } from '@capacitor/network';
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
  _serverAlert: HTMLIonAlertElement | null = null;
  _networkAlert: HTMLIonAlertElement | null = null;
  _clientIdentification: IAppIdentification | null = null;
  dark: boolean = false;

  constructor(
    private _router: Router,
    private _smsService: SmsService,
    private _signalRService: SignalRService,
    private _alertController: AlertController,
    private _toastController: ToastController,
    private _store: Store<RootStoreState.State>,
    private _translationService: TranslocoService,
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

    this._subSink.sink = this._store.select(UIStoreSelectors.StateSelector)
      .subscribe(state => {
        this.dark = state.darkMode;
        this._translationService.setActiveLang(state.language);
      });

    this._subSink.sink = this._store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        if (state.serverInfo?.serverUrl && state.appIdentification?.clientId) {
          this._clientIdentification = state.appIdentification;
          this.setupSignalR(state);
          return;
        }

        // here the app is not configured yet, redirect to the setup page
        this.redirectToSetupPage();
      });

    this._subSink.sink = this._store.select(RootStoreSelectors.ServerConnectionSelector)
      .subscribe(async (status) => {
        if (status == ServerStatus.ONLINE) {
          await this._serverAlert?.dismiss();
          await this.presentToast("you have been connected to the server successfully", 3000);
          setTimeout(async () => { await this._signalRService.sendOnConnectedEvent$(this._clientIdentification.clientId) }, 5000);
          return;
        }

        this._serverAlert = await this._alertController.create({
          backdropDismiss: false,
          message: "failed to connect to server, make sure the server is up, and try again."
        });
        await this._serverAlert.present();
      });

    this._subSink.sink = this._store.select(RootStoreSelectors.NetworkConnectionSelector)
      .subscribe(async status => {
        if (status == DeviceNetworkStatus.OFFLINE) {
          this._networkAlert = await this._alertController.create({
            backdropDismiss: false,
            message: "you have been disconnected, please check your internet connection."
          });
          await this._networkAlert.present();
          return;
        }

        await this._networkAlert?.dismiss();
      });

    // check current network status
    await this.checkCurrentNetworkStatus();
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
    Network.removeAllListeners();
  }

  private setupSignalR(state: State) {
    // init the connection
    this._signalRService.initConnection(state.serverInfo?.serverUrl, state.appIdentification?.clientId)
      .then(() => {
        this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.ONLINE }));
      })
      .catch(async (error) => {
        console.error('server connection failed', error);
        this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE }));
      });

    // register the event for on close
    this._signalRService.onclose(async (error) => {
      console.error('server connection failed', error);
      this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE }));
    });

    // register the handler for the send message event
    this._signalRService.onSendMessageEvent((message: IMessages) => {
      // this._smsService.sendSmsAsync(message.to, message.content);
      console.log("onSendMessageEvent", message);
      this._store.dispatch(MessagesStoreActions.InsertMessage({ message: message }));
    });

    // register the handler for the send message event
    this._signalRService.onClientInfoUpdatedEvent((clientInfo) => {
      console.log("onClientInfoUpdatedEvent", clientInfo);
      this._store.dispatch(SettingsStoreActions.UpdateClientAppIdentification({ data: clientInfo }));
    });

    // register the handler for the send message event
    this._signalRService.onForceDisconnectionEvent((reason) => {
      console.log("onForceDisconnectionEvent", reason);
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
