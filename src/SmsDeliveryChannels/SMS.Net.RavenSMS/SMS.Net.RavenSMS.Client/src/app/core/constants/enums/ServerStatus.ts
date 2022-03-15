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
     * the client application has been forced to disconnect, 
     * either by reconfiguration or by forcing disconnection from the server.
     */
    Disconnected,

    /**
     * we are trying to reconnect to the server
     */
    RECONNECTING,
}
