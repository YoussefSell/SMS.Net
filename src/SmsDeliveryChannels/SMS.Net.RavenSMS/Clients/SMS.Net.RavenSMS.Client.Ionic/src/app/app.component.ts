import { MessagesStoreActions, RootActions, RootStoreSelectors, RootStoreState, StorePersistenceActions, UIStoreSelectors } from './store';
import { DeviceNetworkStatus, DisconnectionReason, MessageStatus, ServerStatus } from './core/constants/enums';
import { SettingsStoreActions, SettingsStoreSelectors } from './store/settings-store';
import { ServerAlertService, SignalRService, SmsService } from './core/services';
import { IAppIdentification, IMessages, IServerInfo } from './core/models';
import { AlertController, ToastController } from '@ionic/angular';
import { Component, OnInit, OnDestroy } from '@angular/core';
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
  _networkAlert: HTMLIonAlertElement | null = null;

  _serverInfo: IServerInfo | null = null;
  _clientIdentification: IAppIdentification | null = null;

  constructor(
    private _router: Router,
    private _alert: AlertController,
    private _toast: ToastController,
    private _smsService: SmsService,
    private _signalRService: SignalRService,
    private _serverAlert: ServerAlertService,
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
      .subscribe(async state => {
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
        // server status is unknown we can't do anything
        if (status == ServerStatus.UNKNOWN) {
          return;
        }

        // if the server is offline we need to notify the user
        if (status === ServerStatus.OFFLINE || status == ServerStatus.RECONNECTING) {
          await this.presentDisconnectedAlertAsync(status);
          return;
        }

        // we are connected to the server, remove the alert if any, and show the success toast
        if (status == ServerStatus.ONLINE) {
          await this.presentToastAsync("you have been connected to the server successfully", 1000);
        }

        await this._serverAlert.dismiss();
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

        this._networkAlert = await this._alert.create({
          backdropDismiss: false,
          message: "you have been disconnected, please check your internet connection."
        });
        await this._networkAlert.present();
      });

    // check current network status
    await this.checkCurrentNetworkStatusAsync();
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
    this._signalRService.onclose(async () => this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE })));

    // register the event for on close
    this._signalRService.onreconnecting(async () => this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.RECONNECTING })));

    // register the handler for the client info updated event
    this._signalRService.onClientInfoUpdatedEvent((clientInfo) => this._store.dispatch(SettingsStoreActions.UpdateClientAppIdentification({ data: clientInfo })));

    // register the handler for the client messages retrieval event
    this._signalRService.onReadClientSentMessagesEvent((messages) => this._store.dispatch(MessagesStoreActions.LoadMessagesFinished({ data: messages })));

    // register the handler for the send message event
    this._signalRService.onSendMessageEvent(async (message: IMessages) => {
      // check for sms permission, if not show an alert to ask the user to allow the permission
      if (!await this._smsService.HasPermissionAsync()) {
        const alertResult = await this._alert.create({
          backdropDismiss: false,
          header: 'Allow "RavenSMS" to send SMS messages',
          message: "RavenSMS requires your permission to send SMS messages, please allow the permission in order for app to work.",
          buttons: [
            {
              text: "Accept",
              role: 'accept',
            },
            {
              text: "Denied",
              role: 'denied',
            }
          ]
        });

        // show the alert
        await alertResult.present();

        // get the result on dismiss
        const dismissResult = await alertResult.onDidDismiss();
        if (dismissResult.role == "denied") {
          // update message status based on the sending result
          this.persistSmsMessage(message, false, 'sms_permission_denied');
          return;
        }
      }

      // send the message
      var result = await this._smsService.sendSmsAsync(message.to, message.content);

      // update message status based on the sending result
      this.persistSmsMessage(message, result.isSuccess, result.error);
    });

    // register the handler for the force disconnect event
    this._signalRService.onClientConnectedEvent(async () => {
      if (this._clientIdentification.clientId) {
        await this._signalRService.sendPersistClientConnectionEvent$(this._clientIdentification.clientId);
        this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.ONLINE }));
        this._store.dispatch(MessagesStoreActions.LoadMessages());
      }
    });

    // register the handler for the force disconnect event
    this._signalRService.onForceDisconnectionEvent(async (reason) => {
      console.log("=> forcing the client to disconnect, reason: ", reason);
      switch (reason) {
        case DisconnectionReason.ClientAlreadyConnected:
          await this.presentToastAsync("this client already connected, please create a new link on the server & connect this client instance to it", 5000);
          await this.disconnectClientAsync();
          break;
        case DisconnectionReason.ClientNotFound:
          await this.presentToastAsync("we couldn't find this client instance on the server, please create a new link on the server & connect this client instance to it", 5000);
          await this.disconnectClientAsync();
          break;
        case DisconnectionReason.ClientDeleted:
          await this.presentToastAsync("the client has been deleted from the server, please create a new link on the server & connect this client instance to it", 5000);
          await this.disconnectClientAsync();
          break;
      }
    });

    // register the handler for the message deletion event
    this._signalRService.onMessageDeletedEvent(async (messageId) => {
      if (this._clientIdentification.clientId) {
        this._store.dispatch(MessagesStoreActions.MessageDeleted({ messageId }));
      }
    });
  }

  /**
   * save the message to the store and update the status of the message on the server
   * @param message the message to persist
   * @param isSuccess if the sending operation has succeed or not
   * @param errorCode an error code if exist
   */
  private persistSmsMessage(message: IMessages, isSuccess: boolean, errorCode?: string): void {
    message = {
      ...message,
      sentOn: new Date(),
      status: isSuccess
        ? MessageStatus.Sent
        : MessageStatus.Failed,
    };

    // insert the message
    this._store.dispatch(MessagesStoreActions.InsertMessage({ message: message }));

    // update the status of the message on the server
    this._signalRService.sendUpdateMessageStatusEventAsync(message.id, message.status, errorCode);
  }

  private async checkCurrentNetworkStatusAsync() {
    const currentNetworkStatus = await Network.getStatus();
    this._store.dispatch(RootActions.UpdateNetworkConnectionStatus({
      newStatus: currentNetworkStatus.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
    }));
  }

  private async presentToastAsync(message: string, duration: number | undefined = undefined) {
    const toast = await this._toast.create({
      duration: duration, message: message,
    });
    toast.present();
  }

  private async presentDisconnectedAlertAsync(status: ServerStatus): Promise<void> {
    // it is not necessary to show the alert if the server info are not set
    // in this case, the user will be redirected to the setup page, so it is enough to return
    if (this._serverInfo == null || this._serverInfo == undefined) {
      return;
    }

    if (status == ServerStatus.RECONNECTING) {
      await this._serverAlert.setMessageAsync("Failed to connect to the server, an automatic reconnection is enabled, try to start the server again.");
    }

    // if the server is offline we will add the possibility to reconfigure the client
    if (status == ServerStatus.OFFLINE) {
      this._serverAlert.setMessageAsync("failed to connect to server, make sure the server is running, and try again.");
      this._serverAlert.setButton('re-configure', 're_configure');
    }

    // we only going to present the alter if it not already presented & we are not on the setup page
    await this._serverAlert.present();

    if (status == ServerStatus.OFFLINE) {
      //if the user choosed to reconfigure the client app, we stop the connection, clear server info & set the status to unknown
      await this._serverAlert.onDidDismissAsync(
        're_configure',
        async () => await this.disconnectClientAsync());
    }
  }

  private async disconnectClientAsync(): Promise<void> {
    await this._signalRService.stop();
    this._store.dispatch(SettingsStoreActions.UpdateServerInfo({ data: null }));
    this._store.dispatch(SettingsStoreActions.UpdateClientAppIdentification({ data: null }));
    this._store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.Disconnected }));
  }
}
