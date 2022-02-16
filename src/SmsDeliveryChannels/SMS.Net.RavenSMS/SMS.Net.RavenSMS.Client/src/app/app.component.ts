import { RootActions, RootStoreSelectors, RootStoreState, StorePersistenceActions, UIStoreSelectors } from './store';
import { SettingsStoreSelectors } from './store/settings-store';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService } from './core/services';
import { ToastController } from '@ionic/angular';
import { Router } from '@angular/router';
import { App } from '@capacitor/app';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { Network } from '@capacitor/network';
import { DeviceNetworkStatus, ServerStatus } from './core/constants/enums';

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

  subSink = new SubSink();
  dark: boolean = false;

  constructor(
    private router: Router,
    //private network: Network,
    public signalRService: SignalRService,
    public toastController: ToastController,
    private store: Store<RootStoreState.State>,
  ) {
    // add a listener on the app state
    App.addListener('appStateChange', ({ isActive }) => {
      if (!isActive) {
        this.store.dispatch(StorePersistenceActions.PersistStore());
      }
    });
  }

  async ngOnInit(): Promise<void> {
    Network.addListener('networkStatusChange', status => {
      this.store.dispatch(RootActions.UpdateNetworkConnectionStatus({
        newStatus: status.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
      }));
    });

    this.subSink.sink = this.store.select(UIStoreSelectors.IsDarkModeSelector)
      .subscribe(value => this.dark = value);

    this.subSink.sink = this.store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        if (state.serverInfo?.serverUrl && state.appIdentification?.clientId) {
          this.signalRService.initConnection(state.serverInfo?.serverUrl, state.appIdentification?.clientId)
            .then(() => {
              this.store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.ONLINE }))
            })
            .catch(async (error) => {
              await this.presentToast('connection  failed' + error + state.serverInfo.serverUrl);
              this.store.dispatch(RootActions.UpdateServerConnectionStatus({ newStatus: ServerStatus.OFFLINE }));
            });
          return;
        }

        // here the app is not configured yet, redirect to the setup page
        this.redirectToSetupPage();
      });

    this.subSink.sink = this.store.select(RootStoreSelectors.ServerConnectionSelector)
      .subscribe(async status => {
        if (status == ServerStatus.ONLINE) {
          await this.presentToast("you have been connected to the server successfully", 3000);
          return;
        }

        //await this.presentToast("failed to connect to server, make sure the server is up, and try again");
      });

    this.subSink.sink = this.store.select(RootStoreSelectors.NetworkConnectionSelector)
      .subscribe(async status => {
        if (status == DeviceNetworkStatus.OFFLINE) {
          await this.presentToast("you have been disconnected, please check your internet connection.");
          return;
        }

        //await this.presentToast("failed to connect to server, make sure the server is up, and try again");
      });

    // check current network status
    await this.checkCurrentNetworkStatus();
  }

  private async checkCurrentNetworkStatus() {
    const currentNetworkStatus = await Network.getStatus();
    this.store.dispatch(RootActions.UpdateNetworkConnectionStatus({
      newStatus: currentNetworkStatus.connected ? DeviceNetworkStatus.ONLINE : DeviceNetworkStatus.OFFLINE
    }));
  }

  ngOnDestroy(): void {
    this.subSink.unsubscribe();
    Network.removeAllListeners();
  }

  redirectToSetupPage(): void {
    this.router.navigateByUrl('/setup');
  }

  async presentToast(message: string, duration: number | undefined = undefined) {
    const toast = await this.toastController.create({
      duration: duration, message: message,
    });
    toast.present();
  }
}
