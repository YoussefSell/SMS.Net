import { MessagesStoreActions, RootActions, RootStoreSelectors, RootStoreState, StorePersistenceActions, UIStoreSelectors } from './store';
import { DeviceNetworkStatus, MessageStatus, ServerStatus } from './core/constants/enums';
import { AlertController, ToastController } from '@ionic/angular';
import { SettingsStoreActions, SettingsStoreSelectors } from './store/settings-store';
import { IAppIdentification, IMessages, IServerInfo } from './core/models';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService, SmsService } from './core/services';
import { TranslocoService } from '@ngneat/transloco';
import { Network } from '@capacitor/network';
import { Router } from '@angular/router';
import { App } from '@capacitor/app';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-root',
  template: `
  <ion-app [class.dark-theme]="_dark">
    <ion-router-outlet id="main-content"></ion-router-outlet>
  </ion-app>
  `,
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {

  _subSink = new SubSink();

  _dark: boolean = false;
  _serverAlert: HTMLIonAlertElement | null = null;
  _networkAlert: HTMLIonAlertElement | null = null;

  _serverInfo: IServerInfo | null = null;
  _clientIdentification: IAppIdentification | null = null;

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

    // add a listener on the network state
    Network.addListener('networkStatusChange', status => {
      this._store.dispatch(RootActions.UpdateNetworkConnectionStatus({
        newStatus: status.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
      }));
    });
  }

  async ngOnInit(): Promise<void> {
    this._subSink.sink = this._store.select(UIStoreSelectors.StateSelector)
      .subscribe(state => {
        this._dark = state.darkMode;
        this._translationService.setActiveLang(state.language);
      });

    this._subSink.sink = this._store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        // save the client & server info
        this._serverInfo = state.serverInfo;
        this._clientIdentification = state.appIdentification;

        // check if the app is connected or not, 
        // if not init the configuration for connection
        if (!this._signalRService.isConnected()) {

          // check if we have a server & client info to establish a connection
          if (state.serverInfo?.serverUrl && state.appIdentification?.clientId) {
            this.setupSignalR(state.serverInfo?.serverUrl, state.appIdentification?.clientId);
            return;
          }

          // here the app is not configured yet, redirect to the setup page
          this._router.navigateByUrl('/setup');
        }
      });

    this._subSink.sink = this._store.select(RootStoreSelectors.ServerConnectionSelector)
      .subscribe(async (status) => {
        // if the server is offline we need to notify the user
        if (status === ServerStatus.OFFLINE || status == ServerStatus.RECONNECTING) {
          await this.presentDisconnectedAlert(status);
        }

        // we are connected to the server, remove the alert if any, and show the success toast
        if (status == ServerStatus.ONLINE) {
          await this._serverAlert?.dismiss();
          this._serverAlert = null;
          await this.presentToast("you have been connected to the server successfully", 1000);
        }
      });

    this._subSink.sink = this._store.select(RootStoreSelectors.NetworkConnectionSelector)
      .subscribe(async status => {
        // if the status is unknown we can't do anything
        if (status === DeviceNetworkStatus.UNKNOWN) {
          return;
        }

        if (status == DeviceNetworkStatus.ONLINE) {
          await this._networkAlert?.dismiss();
          this._networkAlert = null;
          return;
        }

        // check if the network alert is already presented.
        if (this._networkAlert) {
          return;
        }

        this._networkAlert = await this._alertController.create({
          backdropDismiss: false,
          message: "you have been disconnected, please check your internet connection."
        });
        await this._networkAlert.present();
      });

    // check current network status
    await this.checkCurrentNetworkStatus();
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
    Network.removeAllListeners();
  }

  private setupSignalR(serverUrl: string, clientId: string) {
    // init the connection
    this._signalRService.initConnection(serverUrl, clientId)
      .catch(async () => this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE })));

    // register the event for on close
    this._signalRService.onclose(async () =>
      this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE })));

    // register the event for on close
    this._signalRService.onreconnecting(async (error) => {
      if (error) {
        this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.RECONNECTING }));
      }
    });

    // register the handler for the send message event
    this._signalRService.onSendMessageEvent(async (message: IMessages) => {
      var result = await this._smsService.sendSmsAsync(message.to, message.content);

      // if the message sending failed we need to update the status of the message
      if (!result.isSuccess) {
        message = {
          ...message,
          sentOn: new Date(),
          status: MessageStatus.Failed,
        };
      }

      // insert the message
      this._store.dispatch(MessagesStoreActions.InsertMessage({ message: message }));

      // update the status of the message on the server
      this._signalRService.sendUpdateMessageStatusEventAsync(message.id, message.status, result.error);
    });

    // register the handler for the client info updated event
    this._signalRService.onClientInfoUpdatedEvent((clientInfo) =>
      this._store.dispatch(SettingsStoreActions.UpdateClientAppIdentification({ data: clientInfo })));

    // register the handler for the force disconnect event
    this._signalRService.onClientConnectedEvent(async () => {
      if (this._clientIdentification.clientId) {
        await this._signalRService.sendPersistClientConnectionEvent$(this._clientIdentification.clientId);
        this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.ONLINE }));
      }
    });

    // register the handler for the force disconnect event
    this._signalRService.onForceDisconnectionEvent((reason) => {
      console.log("=> onForceDisconnectionEvent", reason);
    });
  }

  private async checkCurrentNetworkStatus() {
    const currentNetworkStatus = await Network.getStatus();
    this._store.dispatch(RootActions.UpdateNetworkConnectionStatus({
      newStatus: currentNetworkStatus.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
    }));
  }

  private async presentToast(message: string, duration: number | undefined = undefined) {
    const toast = await this._toastController.create({
      duration: duration, message: message,
    });
    toast.present();
  }

  private async presentDisconnectedAlert(status: ServerStatus): Promise<void> {
    // it is not necessary to show the alert if the server info are not set
    // in this case, the user will be redirected to the setup page, so it is enough to return
    if (this._serverInfo == null || this._serverInfo == undefined) {
      return;
    }

    // hold the state if we should present the alert or it already presented
    let alreadyPresented = true;

    // check if the server alert is already presented, 
    // if not we should create an instance
    if (this._serverAlert == null) {
      this._serverAlert = await this._alertController.create({ backdropDismiss: false });
      alreadyPresented = false;
    }

    if (status == ServerStatus.RECONNECTING) {
      this._serverAlert.message = "Failed to connect to the server, an automatic reconnection is enabled, try to start the server again.";
    }

    // if the server is offline we will add the possibility to reconfigure the client
    if (status == ServerStatus.OFFLINE) {
      this._serverAlert.message = "failed to connect to server, make sure the server is running, and try again.";
      this._serverAlert.buttons = [{ text: 're-configure', role: 're_configure' }];
    }

    if (!alreadyPresented) {
      await this._serverAlert.present();
    }

    if (status == ServerStatus.OFFLINE) {
      var dismissResult = await this._serverAlert.onDidDismiss();

      //if the user choosed to reconfigure the client app, we stop the connection, clear server info & set the status to unknown
      if (dismissResult.role == 're_configure') {
        await this._signalRService.stop();
        this._store.dispatch(SettingsStoreActions.UpdateServerInfo({ data: null }));
        this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.UNKNOWN }));
      }
    }
  }
}
