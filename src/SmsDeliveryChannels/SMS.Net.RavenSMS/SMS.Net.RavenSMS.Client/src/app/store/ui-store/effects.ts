import { Injectable } from '@angular/core';
import { Actions } from '@ngrx/effects';

@Injectable()
export class MainEffects {
  constructor(
    private actions$: Actions,
  ) { }
}
