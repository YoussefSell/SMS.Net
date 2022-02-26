import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessagesStoreActions, MessagesStoreSelectors, RootStoreState } from 'src/app/store';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { IMessages } from 'src/app/core/models';

@Component({
  selector: 'page-message-detail',
  styleUrls: ['./detail.page.scss'],
  templateUrl: 'detail.page.html'
})
export class DetailPage implements OnDestroy {
  _subSink = new SubSink();
  _message: IMessages | null = null;

  session: any;
  isFavorite = false;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _store: Store<RootStoreState.State>,
  ) { }

  ionViewWillEnter() {
    this._subSink.sink = this._route.params
      .subscribe(params => {
        const messageId = params['messageId'];
        if (messageId) {
          this._store.dispatch(MessagesStoreActions.SelectMessage({ messageId }));
          return;
        }

        this.navigateBack();
      });

    this._subSink.sink = this._store.select(MessagesStoreSelectors.SelectedMessageSelector)
      .subscribe(message => {
        if (message) {
          this._message = message;
          return;
        }

        this.navigateBack();
      });
  }

  ngOnDestroy(): void {
    this._store.dispatch(MessagesStoreActions.UnselectMessage());
    this._subSink.unsubscribe();
  }

  navigateBack(): void {
    this._router.navigateByUrl(`/app/tabs/messages`);
  }

  removeMessage(): void {
    this._store.dispatch(MessagesStoreActions.DeleteMessage({
      messageId: this._message.id
    }));
  }
}
