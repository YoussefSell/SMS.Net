import { createReducer, on } from '@ngrx/store';
import { ServerStatus } from 'src/app/core/constants/enums';
import * as Actions from './actions';
import { State } from './state';

/**
 * set the initial state of the settings module
 */
const initialState: State = {
    appIdentification: null,
    serverInfo: null,
};

/**
 * the main ui module reducer
 */
export const MainReducer = createReducer<State>(
    // set the initial state
    initialState,

    // check the actions
    on(Actions.UpdateServerStatus, (state, action): State => {
        return {
            ...state,
            serverInfo: {
                ...state.serverInfo,
                status: action.newStatus
            },
        };
    }),
    on(Actions.UpdateServerInfo, (state, action): State => {
        return {
            ...state,
            serverInfo: {
                ...action.data
            },
        };
    }),
    on(Actions.UpdateClientAppIdentification, (state, action): State => {
        return {
            ...state,
            appIdentification: {
                ...action.data
            },
        };
    }),
    on(Actions.ConfigureClient, (state, action): State => {
        return {
            ...state,
            appIdentification: {
                clientId: action.data.clientId,
                clientName: "",
                clientDescription: "",
                clientPhoneNumber: "",
            },
            serverInfo: {
                status: ServerStatus.ONLINE,
                serverUrl: action.data.serverUrl,
            }
        };
    }),
);
