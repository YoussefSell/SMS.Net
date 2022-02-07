import { IMessages } from 'src/app/core/models';

/**
 * this interface defines the messages module state
 */
export interface State {
    /**
     * the list of the message
     */
    messages: ReadonlyArray<IMessages>;

    /**
     * the currently selected message
     */
    selectedMessage: Readonly<IMessages> | null;

    /**
     * the error associated with the action of the message store
     */
    error: Readonly<any> | null;

    /**
     * is currently performing an action
     */
    isLoading: boolean;
}
