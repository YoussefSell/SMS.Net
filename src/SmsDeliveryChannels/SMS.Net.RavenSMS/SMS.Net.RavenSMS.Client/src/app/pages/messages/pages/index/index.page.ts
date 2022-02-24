import { AlertController, IonList, IonRouterOutlet, LoadingController, ModalController, ToastController, Config } from '@ionic/angular';
import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { MessagesStoreSelectors, RootStoreState } from 'src/app/store';
import { IMessages } from 'src/app/core/models';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import * as moment from 'moment';
import * as _ from 'lodash';

@Component({
  selector: 'page-messages',
  templateUrl: 'index.page.html',
  styleUrls: ['./index.page.scss'],
})
export class IndexPage implements OnInit, OnDestroy {
  _subSink = new SubSink();

  // Gets a reference to the list element
  @ViewChild('scheduleList', { static: true }) scheduleList: IonList;

  ios: boolean;
  dayIndex = 0;
  queryText = '';
  segment = 'all';
  excludeTracks: any = [];
  showMessages: any = [];
  groups: any = [];
  confDate: string;
  showSearchbar: boolean;

  _messagesGroups: { date: string; messages: IMessages[] }[] = [];

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
        // group messages by date
        const grouping = _.groupBy(messages, item => moment(item.date).format('YYYY-MM-DD'));

        // transform the grouping into an array
        this._messagesGroups = Object.keys(grouping)
          .map(key => ({ date: key, messages: grouping[key] }))

        console.log('messages', this._messagesGroups);
      });

    this.updateSchedule();

    this.ios = this.config.get('mode') === 'ios';
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
  }

  removeMessage(): void {
    console.log('remove to favorite clicked')
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
