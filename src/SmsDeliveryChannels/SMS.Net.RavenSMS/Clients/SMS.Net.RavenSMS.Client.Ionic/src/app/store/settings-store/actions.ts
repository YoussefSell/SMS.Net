import { IAppIdentification, IQrContentModel, IServerInfo } from 'src/app/core/models';
import { createAction, props } from '@ngrx/store';

/**
 * this enums defines the action types for the ui module
 */
export enum StoreActionTypes {
    CONFIGURE_CLIENT = "@setting/client/configure",
    UPDATE_SERVER_INFO = '@settings/server/info/update',
    UPDATE_CLIENT_APP_IDENTIFICATION = '@settings/client/identification/update',
}

export const ConfigureClient = createAction(StoreActionTypes.CONFIGURE_CLIENT, props<{ data: IQrContentModel }>());
export const UpdateServerInfo = createAction(StoreActionTypes.UPDATE_SERVER_INFO, props<{ data: IServerInfo | null }>());
export const UpdateClientAppIdentification = createAction(StoreActionTypes.UPDATE_CLIENT_APP_IDENTIFICATION, props<{ data: IAppIdentification }>());
