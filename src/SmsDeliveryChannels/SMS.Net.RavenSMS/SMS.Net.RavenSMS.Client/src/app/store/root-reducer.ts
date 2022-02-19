import { DeviceNetworkStatus, ServerStatus } from '../core/constants/enums';
import { createReducer, on } from '@ngrx/store';
import * as Actions from './root-actions';
import { State } from './root-state';

/**
 * set the initial state of the ui module
 */
const initialState: State = {
    serverConnection: ServerStatus.ONLINE,
    networkConnection: DeviceNetworkStatus.ONLINE,
};

/**
 * the main ui module reducer
 */
export const app = createReducer<State>(
    // set the initial state
    initialState,

    // check the actions
    on(Actions.UpdateNetworkConnectionStatus, (state, action): State => {
        return {
            ...state,
            networkConnection: action.newStatus,
        };
    }),
    on(Actions.UpdateServerConnectionStatus, (state, action): State => {
        return {
            ...state,
            serverConnection: action.newStatus,
        };
    }),
);
