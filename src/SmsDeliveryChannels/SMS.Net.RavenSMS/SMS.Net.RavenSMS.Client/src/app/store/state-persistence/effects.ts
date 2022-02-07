import { distinctUntilChanged, exhaustMap, map, switchMap } from 'rxjs/operators';
import { createEffect, Actions, ofType, OnInitEffects } from '@ngrx/effects';
import { StorageService } from 'src/app/core/services';
import { Action, Store } from '@ngrx/store';
import { Injectable } from '@angular/core';
import * as ActionsTypes from './actions';
import { State } from '../root-state';
import { from } from 'rxjs';

@Injectable()
export class StorePersistenceEffects implements OnInitEffects {

    constructor(
        private _storage: StorageService,
        private store: Store<State>,
        private action$: Actions
    ) { }

    loadState$ = createEffect(() => {
        return this.action$.pipe(
            ofType(ActionsTypes.LoadState),
            exhaustMap(() => {
                return from(this._storage.initAsync()).pipe(() => {
                    return this._storage.getState$<State>()
                        .pipe(
                            map(state => {
                                if (state) {
                                    return ActionsTypes.StateLoadingSucceeded({ state });
                                }

                                return ActionsTypes.StateLoadingFailed();
                            }))
                });
            })
        );
    });


    persistState$ = createEffect(() => {
        return this.action$.pipe(
            ofType(ActionsTypes.PersistStore),
            switchMap(() => this.store),
            distinctUntilChanged(),
            exhaustMap((state) => {
                return from(this._storage.saveState$<State>(state));
            })
        );
    },
        { dispatch: false });

    ngrxOnInitEffects(): Action {
        return ActionsTypes.LoadState();
    }
}
