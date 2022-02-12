/**
 * the interface that defines the qr code content
 */

export interface IQrContentModel {
    /**
     * the server url
     */
    serverUrl: string;

    /**
     * the client id
     */
    clientId: string;

    /**
     * the client name
     */
    clientName: string;

    /**
     * the client description
     */
    clientDescription: string;
}
