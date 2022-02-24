import { MessageStatus } from "../constants/enums";

/**
 * interface that defines the sms message
 */
export interface IMessages {
    /**
     * the id of the message.
     */
    id: string;

    /**
     * the phone number that the message is sent to.
     */
    to: string;

    /**
     * the phone number of from 
     */
    from: string;

    /**
     * the content of the message
     */
    content: string;

    /**
     * the date the message is sent
     */
    date: Date;

    /**
     * the status of the message
     */
    status: MessageStatus;
}
