import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as ActionTypes from './actions';
import { tap } from 'rxjs';

@Injectable()
export class MainEffects {

  constructor(
    private router: Router,
    private actions$: Actions,
  ) { }

  onClientConfigured$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(ActionTypes.ConfigureClient),
      tap((props) => {
        this.router.navigateByUrl("/", { replaceUrl: true });
      })
    );
  }, { dispatch: false });
}
