import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController, IonList, IonRouterOutlet, LoadingController, ModalController, ToastController, Config } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { IMessages } from 'src/app/core/models';
import { MessagesStoreSelectors, RootStoreState } from 'src/app/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'page-messages',
  templateUrl: 'messages.html',
  styleUrls: ['./messages.scss'],
})
export class MessagesPage implements OnInit, OnDestroy {
  _subSink = new SubSink();

  // Gets a reference to the list element
  @ViewChild('scheduleList', { static: true }) scheduleList: IonList;

  ios: boolean;
  dayIndex = 0;
  queryText = '';
  segment = 'all';
  excludeTracks: any = [];
  shownSessions: any = [];
  groups: any = [];
  confDate: string;
  showSearchbar: boolean;

  _messages: ReadonlyArray<IMessages> = [];

  constructor(
    public alertCtrl: AlertController,
    public loadingCtrl: LoadingController,
    public modalCtrl: ModalController,
    public router: Router,
    public routerOutlet: IonRouterOutlet,
    public toastCtrl: ToastController,
    public config: Config,
    private _store: Store<RootStoreState.State>,
  ) { }

  ngOnInit() {
    this._subSink.sink = this._store.select(MessagesStoreSelectors.MessagesSelector)
      .subscribe(messages => {
        this._messages = messages;
        console.log('messages', messages);
      });

    this.updateSchedule();

    this.ios = this.config.get('mode') === 'ios';
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
  }

  updateSchedule() {
    // Close any open sliding items when the schedule updates
    if (this.scheduleList) {
      this.scheduleList.closeSlidingItems();
    }

  }

  async openSocial(network: string, fab: HTMLIonFabElement) {
    const loading = await this.loadingCtrl.create({
      message: `Posting to ${network}`,
      duration: (Math.random() * 1000) + 500
    });
    await loading.present();
    await loading.onWillDismiss();
    fab.close();
  }
}
