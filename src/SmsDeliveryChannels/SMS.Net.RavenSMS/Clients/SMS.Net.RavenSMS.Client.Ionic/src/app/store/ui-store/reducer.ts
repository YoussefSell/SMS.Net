import { createReducer, on } from '@ngrx/store';
import * as Actions from './actions';
import { State } from './state';

/**
 * set the initial state of the ui module
 */
const initialState: State = {
    darkMode: false,
    language: 'en',
};

/**
 * the main ui module reducer
 */
export const MainReducer = createReducer<State>(
    // set the initial state
    initialState,

    // check the actions
    on(Actions.updateDarkMode, (state, action): State => {
        return {
            ...state,
            darkMode: action.value,
        };
    }),
    on(Actions.updateLanguage, (state, action): State => {
        return {
            ...state,
            language: action.value,
        };
    }),
);
