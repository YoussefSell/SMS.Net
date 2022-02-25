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

  _messages: ReadonlyArray<IMessages> = [];
  _filteredMessages: ReadonlyArray<IMessages> = [];
  _messagesGroups: { date: string; messages: IMessages[] }[] = [];

  constructor(
    private _config: Config,
    private _store: Store<RootStoreState.State>,
  ) { }

  ngOnInit() {
    this._subSink.sink = this._store.select(MessagesStoreSelectors.MessagesSelector)
      .subscribe(messages => {
        this._filteredMessages = messages;
        this._messages = messages;
        this.groupMessages();
      });

    this._is_ios = this._config.get('mode') === 'ios';
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
  }

  groupMessages(): void {
    // group messages by date
    const grouping = _.groupBy(this._filteredMessages, item => moment(item.date).format('YYYY-MM-DD'));

    // transform the grouping into an array
    this._messagesGroups = Object.keys(grouping)
      .map(key => ({ date: key, messages: grouping[key] }));
  }

  removeMessage(message: IMessages): void {
    this._store.dispatch(MessagesStoreActions.DeleteMessage({ messageId: message.id }));
  }

  search(): void {
    if (this._searchQuery && this._searchQuery.length > 0) {
      // perform search
      this._filteredMessages = this._messages.filter(message =>
        message.content.includes(this._searchQuery)
        || message.to.includes(this._searchQuery));

      // group messages
      this.groupMessages();
      return;
    }

    // reset the list & perform the grouping
    this._filteredMessages = this._messages;
    this.groupMessages();
  }
}
