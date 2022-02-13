import { createAction, props } from '@ngrx/store';
import { DeviceNetworkStatus, ServerStatus } from '../core/constants/enums';

/**
 * this enums defines the root action types
 */
export enum RootActionTypes {
    NO_ACTION = '@root/empty',

    UPDATE_SERVER_CONNECTION_STATUS = "@root/update/server/connection",
    UPDATE_NETWORK_CONNECTION_STATUS = "@root/update/network/connection",
}

export const NoAction = createAction(RootActionTypes.NO_ACTION);

export const UpdateServerConnectionStatus = createAction(RootActionTypes.UPDATE_SERVER_CONNECTION_STATUS, props<{ newStatus: ServerStatus }>());
export const UpdateNetworkConnectionStatus = createAction(RootActionTypes.UPDATE_NETWORK_CONNECTION_STATUS, props<{ newStatus: DeviceNetworkStatus }>());
