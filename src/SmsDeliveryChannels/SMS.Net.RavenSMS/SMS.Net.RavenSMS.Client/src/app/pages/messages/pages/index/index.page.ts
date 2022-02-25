import { AlertController, IonList, IonRouterOutlet, LoadingController, ModalController, ToastController, Config } from '@ionic/angular';
import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { MessagesStoreActions, MessagesStoreSelectors, RootStoreState } from 'src/app/store';
import { IMessages } from 'src/app/core/models';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import * as moment from 'moment';
import * as _ from 'lodash';
import { MessageStatus } from 'src/app/core/constants/enums';

@Component({
  selector: 'page-messages',
  templateUrl: 'index.page.html',
  styleUrls: ['./index.page.scss'],
})
export class IndexPage implements OnInit, OnDestroy {
  _subSink = new SubSink();

  _is_ios: boolean;
  _showSearchbar: boolean;
  _searchQuery: string;
  _messageStatus = MessageStatus;
  _messagesGroups: { date: string; messages: IMessages[] }[] = [];

  constructor(
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

    this._is_ios = this.config.get('mode') === 'ios';
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
  }

  removeMessage(message: IMessages): void {
    this._store.dispatch(MessagesStoreActions.DeleteMessage({ messageId: message.id }));
  }

  search(): void {
    console.log(this._searchQuery);
  }
}
