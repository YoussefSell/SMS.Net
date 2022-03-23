import { createEffect, Actions, ofType } from '@ngrx/effects';
import { SettingsStoreSelectors } from '../settings-store';
import { exhaustMap, withLatestFrom } from 'rxjs/operators';
import { SignalRService } from 'src/app/core/services';
import { Injectable } from '@angular/core';
import * as ActionTypes from './actions';
import { Store } from '@ngrx/store';
import { State } from './state';

@Injectable()
export class MainEffects {

  constructor(
    private store: Store<State>,
    private _signalRService: SignalRService,
    private actions$: Actions,
  ) { }

  LoadMessages$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(ActionTypes.LoadMessages),
      withLatestFrom(this.store.select(SettingsStoreSelectors.AppIdentificationSelector)),
      exhaustMap(async ([, clientInfo]) => {
        if (clientInfo) {
          await this._signalRService.loadClientMessagesAsync(clientInfo.clientId)
        }
      })
    );
  }, { dispatch: false });
}
