import { IAppIdentification, IServerInfo } from "src/app/core/models";

/**
 * this interface defines the authentication module state
 */
export interface State {
    /**
     * the app Identification state
     */
    appIdentification: IAppIdentification | null;

    /**
     * the server info state
     */
    serverInfo: IServerInfo | null;
}
