import { createAction, props } from '@ngrx/store';
import { MessageStatus } from 'src/app/core/constants/enums';
import { IMessages } from 'src/app/core/models';

/**
 * this enums defines the action types for the companies module
 */
export enum StoreActionTypes {
    LOAD_MESSAGES = '@messages/load',
    LOAD_MESSAGES_FINISHED = '@company/load/finished',
    SELECT_MESSAGE = '@messages/select',
    INSERT_MESSAGE = '@messages/insert',
    UPDATE_MESSAGE_STATUS = '@messages/update/status',
}

export const LoadMessages = createAction(StoreActionTypes.LOAD_MESSAGES);
export const LoadMessagesFinished = createAction(StoreActionTypes.LOAD_MESSAGES_FINISHED, props<{ data: IMessages[] }>());

export const SelectMessage = createAction(StoreActionTypes.LOAD_MESSAGES, props<{ messageId: string; }>());

export const InsertMessage = createAction(StoreActionTypes.INSERT_MESSAGE, props<{ message: IMessages }>());
export const UpdateMessageStatus = createAction(StoreActionTypes.UPDATE_MESSAGE_STATUS, props<{ messageId: string, newStatus: MessageStatus }>());
