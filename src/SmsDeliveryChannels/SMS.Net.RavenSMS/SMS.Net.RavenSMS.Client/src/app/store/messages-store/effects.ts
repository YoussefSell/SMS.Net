import { createEffect, Actions, ofType } from '@ngrx/effects';
import { map, catchError, exhaustMap } from 'rxjs/operators';
import { MessagesService } from 'src/app/core/services';
import { Injectable } from '@angular/core';
import * as ActionTypes from './actions';
import { Store } from '@ngrx/store';
import { State } from './state';
import { of } from 'rxjs';

@Injectable()
export class MainEffects {

  constructor(
    private service: MessagesService,
    private store: Store<State>,
    private actions$: Actions,
  ) { }

  LoadMessages$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(ActionTypes.LoadMessages),
      exhaustMap((props) => {
        return this.service.loadMessages$()
          .pipe(
            map(result => {
              return ActionTypes.LoadMessagesFinished({ data: result });
            }),
            catchError(requestError => {
              return of(ActionTypes.LoadMessagesFinished({
                data: []
              }));
            })
          );
      })
    );
  });

}
