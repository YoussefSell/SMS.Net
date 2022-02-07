import { createReducer, on } from '@ngrx/store';
import { IMessages } from 'src/app/core/models';
import * as Actions from './actions';
import { State } from './state';

/**
 * set the initial state of the authentication module
 */
const initialState: State = {
    isLoading: false,
    error: null,
    messages: [],
    selectedMessage: null,
};

/**
 * the authentication module reducer
 */
export const MainReducer = createReducer<State>(
    // set the initial state
    initialState,

    // check the actions
    on(Actions.LoadMessages, (state, action): State => {
        return {
            ...state,
            isLoading: true,
        };
    }),
    on(Actions.LoadMessagesFinished, (state, action): State => {
        return {
            ...state,
            messages: action.data as IMessages[]
        };
    }),
);
