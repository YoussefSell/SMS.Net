/**
 * the server status
 */
export enum ServerStatus {
    /**
     * the server connection status is unknown.
     */
    UNKNOWN,

    /**
     * the server is up and running
     */
    ONLINE,

    /**
     * the server is down/offline
     */
    OFFLINE,

    /**
     * we are trying to reconnect to the server
     */
    RECONNECTING,
}
