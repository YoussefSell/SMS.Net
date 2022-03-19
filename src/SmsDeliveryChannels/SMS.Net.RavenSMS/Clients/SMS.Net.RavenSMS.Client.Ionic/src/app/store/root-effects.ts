import { Injectable, } from '@angular/core';
import { Actions } from '@ngrx/effects';
import { State } from './root-state';
import { Store } from '@ngrx/store';

@Injectable()
export class RootEffects {

    constructor(
        private store: Store<State>,
        private actions$: Actions,
    ) { }

}
