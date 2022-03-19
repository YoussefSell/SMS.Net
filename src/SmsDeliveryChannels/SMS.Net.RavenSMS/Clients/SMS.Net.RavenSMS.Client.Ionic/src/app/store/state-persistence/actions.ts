import { createAction, props } from '@ngrx/store';
import { State } from '../root-state';

/**
 * this enums defines the action types for the Store Persistence module
 */
export enum StorePersistenceActionTypes {
    LOAD = '@store/load/state',
    LOAD_FAILED = '@store/load/state/failed',
    LOAD_SUCCEEDED = '@store/load/state/succeeded',

    PERSIST = '@store/persist',
    PERSISTENCE_FAILURE = '@store/persist/failed',
    PERSISTENCE_SUCCESS = '@store/persist/succeed',
}

export const LoadState = createAction(StorePersistenceActionTypes.LOAD);
export const StateLoadingFailed = createAction(StorePersistenceActionTypes.LOAD_FAILED);
export const StateLoadingSucceeded = createAction(StorePersistenceActionTypes.LOAD_SUCCEEDED, props<{ state: State }>());

export const PersistStore = createAction(StorePersistenceActionTypes.PERSIST);
export const StorePersistenceFailed = createAction(StorePersistenceActionTypes.PERSISTENCE_FAILURE);
export const StorePersistenceSucceed = createAction(StorePersistenceActionTypes.PERSISTENCE_SUCCESS);
