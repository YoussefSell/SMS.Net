import { createReducer, on } from '@ngrx/store';
import { IMessages } from 'src/app/core/models';
import * as Actions from './actions';
import { State } from './state';

/**
 * set the initial state of the authentication module
 */
const initialState: State = {
    isLoading: false,
    error: null,
    messages: [],
    selectedMessage: null,
};

/**
 * the authentication module reducer
 */
export const MainReducer = createReducer<State>(
    // set the initial state
    initialState,

    // check the actions
    on(Actions.LoadMessages, (state, action): State => {
        return {
            ...state,
            isLoading: true,
        };
    }),
    on(Actions.LoadMessagesFinished, (state, action): State => {
        return {
            ...state,
            messages: action.data as IMessages[]
        };
    }),
    on(Actions.SelectMessage, (state, action): State => {
        const message = state.messages.find(m => m.id == action.messageId);

        // if we found a message set as the selected message.
        if (message !== null && message !== undefined) {
            return {
                ...state,
                selectedMessage: message,
            }
        }

        return state;
    }),
    on(Actions.InsertMessage, (state, action): State => {
        return {
            ...state,
            messages: [
                action.message,
                ...state.messages
            ]
        };
    }),
    on(Actions.UpdateMessageStatus, (state, action): State => {
        // get the message index
        const messageIndex = state.messages.map(m => m.id).indexOf(action.messageId);

        let newState: State = {
            ...state,
            messages: [
                ...state.messages.slice(0, messageIndex),
                {
                    ...state.messages[messageIndex],
                    status: action.newStatus,
                },
                ...state.messages.slice(messageIndex + 1),
            ]
        };

        if (state.selectedMessage && state.selectedMessage.id == action.messageId) {
            newState.selectedMessage = {
                ...state.selectedMessage,
                status: action.newStatus,
            }
        }

        return newState;
    }),
);
