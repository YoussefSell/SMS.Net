import { DeviceNetworkStatus, ServerStatus } from '../core/constants/enums';

/**
 * the root state of the application
 */
export interface State {
    /**
     * the server connection status
     */
    serverConnection: ServerStatus;

    /**
     * the network connection status
     */
    networkConnection: DeviceNetworkStatus;
}
