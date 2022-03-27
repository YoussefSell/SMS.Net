/**
 * the message status
 */
export enum MessageStatus {
    /**
     *  default status.
     */
    None = -1,

    /**
     *  the message has been created.
     */
    Created = 0,

    /**
     *  the message has been Queued.
     */
    Queued = 1,

    /**
     *  failed to send the message.
     */
    Failed = 2,

    /**
     *  the message has been sent successfully.
     */
    Sent = 3
}
