import { IAppIdentification, IServerInfo } from 'src/app/core/models';
import { ServerStatus } from 'src/app/core/constants/enums';
import { createAction, props } from '@ngrx/store';

/**
 * this enums defines the action types for the ui module
 */
export enum StoreActionTypes {
    UPDATE_SERVER_INFO = '@settings/server/info/update',
    UPDATE_SERVER_STATUS = '@settings/server/status/update',
    UPDATE_CLIENT_APP_IDENTIFICATION = '@settings/client/identification/update',
}

export const UpdateServerStatus = createAction(StoreActionTypes.UPDATE_SERVER_STATUS, props<{ newStatus: ServerStatus }>());

export const UpdateServerInfo = createAction(StoreActionTypes.UPDATE_SERVER_INFO, props<{ data: IServerInfo }>());
export const UpdateClientAppIdentification = createAction(StoreActionTypes.UPDATE_SERVER_STATUS, props<{ data: IAppIdentification }>());
