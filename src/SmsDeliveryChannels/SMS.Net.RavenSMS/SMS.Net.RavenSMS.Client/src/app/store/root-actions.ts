import { createAction } from '@ngrx/store';

/**
 * this enums defines the root action types
 */
export enum RootActionTypes {
    NO_ACTION = '@root/empty',
}

export const NoAction = createAction(RootActionTypes.NO_ACTION);
