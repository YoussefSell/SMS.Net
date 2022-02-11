/**
 * defines the client app identification details
 */
export interface IAppIdentification {
    /**
     * the unique identifier of this application app on the server
     */
    clientId: string;

    /**
     * the name of the client application
     */
    clientName: string;

    /**
     * the simple description of the client application
     */
    clientDescription: string;
}
