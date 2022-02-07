import { createAction, props } from '@ngrx/store';

/**
 * this enums defines the action types for the ui module
 */
export enum StoreActionTypes {
    TOGGLE_DARK_MODE = '@ui/dark_mode/toggle',
}

export const toggleDarkMode = createAction(StoreActionTypes.TOGGLE_DARK_MODE);
