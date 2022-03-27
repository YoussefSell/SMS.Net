import { MessageStatus } from 'src/app/core/constants/enums';
import { createAction, props } from '@ngrx/store';
import { IMessages } from 'src/app/core/models';

/**
 * this enums defines the action types for the companies module
 */
export enum StoreActionTypes {
    LOAD_MESSAGES = '@messages/load',
    LOAD_MESSAGES_FINISHED = '@messages/load/finished',

    SELECT_MESSAGE = '@messages/select',
    UNSELECT_MESSAGE = '@messages/unselect',

    INSERT_MESSAGE = '@messages/insert',
    UPDATE_MESSAGE_STATUS = '@messages/update/status',
    DELETE_MESSAGE = '@messages/delete',
    MESSAGE_DELETED = '@messages/deleted',
}

export const LoadMessages = createAction(StoreActionTypes.LOAD_MESSAGES);
export const LoadMessagesFinished = createAction(StoreActionTypes.LOAD_MESSAGES_FINISHED, props<{ data: IMessages[] }>());

export const SelectMessage = createAction(StoreActionTypes.SELECT_MESSAGE, props<{ messageId: string; }>());
export const UnselectMessage = createAction(StoreActionTypes.UNSELECT_MESSAGE);

export const InsertMessage = createAction(StoreActionTypes.INSERT_MESSAGE, props<{ message: IMessages }>());
export const UpdateMessageStatus = createAction(StoreActionTypes.UPDATE_MESSAGE_STATUS, props<{ messageId: string, newStatus: MessageStatus }>());
export const DeleteMessage = createAction(StoreActionTypes.DELETE_MESSAGE, props<{ messageId: string; }>());
export const MessageDeleted = createAction(StoreActionTypes.DELETE_MESSAGE, props<{ messageId: string; }>());

