import { createAction, props } from '@ngrx/store';
import { IMessages } from 'src/app/core/models';

/**
 * this enums defines the action types for the companies module
 */
export enum StoreActionTypes {
    LOAD_MESSAGES = '@messages/load',
    LOAD_MESSAGES_FINISHED = '@company/load/finished',

    SELECT_MESSAGE = '@messages/select',
    SELECT_MESSAGE_FINISHED = '@messages/select',
}

export const LoadMessages = createAction(StoreActionTypes.LOAD_MESSAGES);
export const LoadMessagesFinished = createAction(StoreActionTypes.LOAD_MESSAGES_FINISHED, props<{ data: IMessages[] }>());

export const SelectMessages = createAction(StoreActionTypes.LOAD_MESSAGES, props<{ messageId: string; }>());
export const SelectMessagesFinished = createAction(StoreActionTypes.LOAD_MESSAGES_FINISHED, props<{ messageId: string; data: IMessages }>());
