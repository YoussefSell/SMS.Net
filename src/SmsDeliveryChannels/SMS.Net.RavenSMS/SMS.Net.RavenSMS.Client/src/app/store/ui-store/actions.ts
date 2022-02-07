import { createAction, props } from '@ngrx/store';

/**
 * this enums defines the action types for the ui module
 */
export enum StoreActionTypes {
    UPDATE_DARK_MODE = '@ui/dark_mode/update',
}

export const updateDarkMode = createAction(StoreActionTypes.UPDATE_DARK_MODE, props<{ value: boolean }>());
