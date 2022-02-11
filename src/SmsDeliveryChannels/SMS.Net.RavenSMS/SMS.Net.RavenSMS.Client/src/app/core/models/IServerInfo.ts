import { ServerStatus } from "../constants/enums";

/**
 * define the server details
 */
export interface IServerInfo {
    /**
     * the server url
     */
    serverUrl: string;

    /**
     * the status of the server
     */
    status: ServerStatus;
}
