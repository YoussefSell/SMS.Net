import { createReducer, on } from '@ngrx/store';
import * as Actions from './actions';
import { State } from './state';

/**
 * set the initial state of the ui module
 */
const initialState: State = {
    darkMode: false,
};

/**
 * the main ui module reducer
 */
export const MainReducer = createReducer<State>(
    // set the initial state
    initialState,

    // check the actions
    on(Actions.toggleDarkMode, (state, action): State => {
        return {
            ...state,
            darkMode: !state.darkMode,
        };
    }),
);
