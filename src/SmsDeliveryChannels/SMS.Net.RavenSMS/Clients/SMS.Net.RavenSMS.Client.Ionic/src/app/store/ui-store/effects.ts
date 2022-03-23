import { createEffect, Actions, ofType } from '@ngrx/effects';
import { TranslocoService } from '@ngneat/transloco';
import * as RootActionTypes from '../root-actions';
import { exhaustMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import * as ActionTypes from './actions';
import { of } from 'rxjs';

@Injectable()
export class MainEffects {

  constructor(
    private actions$: Actions,
    private translationService: TranslocoService,
  ) { }

  updateLanguage$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(ActionTypes.updateLanguage),
      exhaustMap((props) => {
        this.translationService.setActiveLang(props.value);
        return of(RootActionTypes.NoAction());
      })
    );
  },
    { dispatch: false });
}
