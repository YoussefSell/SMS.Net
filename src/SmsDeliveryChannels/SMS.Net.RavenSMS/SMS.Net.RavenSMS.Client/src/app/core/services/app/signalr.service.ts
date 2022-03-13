import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { IAppIdentification, IMessages } from '../../models';
import { DisconnectionReason, MessageStatus } from '../../constants/enums';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class SignalRService {

    private serverSuffix = "RavenSMS/Hub";
    private hubConnection: HubConnection;

    /**
     * build the instance of the HubConnection
     * @param serverUrl the ravenSMS server url
     */
    private buildHubConnection(serverUrl: string, clientId: string): void {
        if (serverUrl == null || serverUrl == undefined) {
            throw new Error('the server url is not specified');
        }

        // build the server url
        let url = serverUrl.endsWith('/')
            ? serverUrl + this.serverSuffix
            : serverUrl + '/' + this.serverSuffix;

        // attach the clientId asa queryString
        url += `?clientId=${clientId}`

        // build the connection hub
        this.hubConnection = new HubConnectionBuilder()
            .withAutomaticReconnect([
                100, 200, 400, 800,
                1000, 1200, 1400, 1800,
                2000, 2200, 2400, 2800,
                3000, 3200, 3400, 3800,
            ])
            .withUrl(url)
            .build();
    }

    /**
     * start the server connection
     */
    private startConnectionAsync(): Promise<void> {
        return this.hubConnection.start();
    }

    // init the server connection
    public initConnection(serverUrl: string, clientId: string): Promise<void> {
        // 1- build the connection hub
        this.buildHubConnection(serverUrl, clientId);

        // 2- start the connection
        return this.startConnectionAsync();
    }

    /**
     * check if the hub is connected to the server
     * @returns true if connected false if not
     */
    public isConnected(): boolean {
        return this.hubConnection ? this.hubConnection.state === HubConnectionState.Connected : false;
    }

    /** Registers a handler that will be invoked when the connection is closed.
     *
     * @param {Function} callback The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).
     */
    public onclose(callback: (error?: Error) => void): void {
        this.hubConnection.onclose(callback);
    }

    /** Registers a handler that will be invoked when the connection successfully reconnects.
     *
     * @param {Function} callback The handler that will be invoked when the connection successfully reconnects.
     */
    public onreconnected(callback: (connectionId?: string) => void): void {
        this.hubConnection.onreconnected(callback);
    }

    /** Registers a handler that will be invoked when the connection starts reconnecting.
     *
     * @param {Function} callback The handler that will be invoked when the connection starts reconnecting. Optionally receives a single argument containing the error that caused the connection to start reconnecting (if any).
     */
    public onreconnecting(callback: (error?: Error) => void): void {
        this.hubConnection.onreconnecting(callback);
    }

    async sendUpdateMessageStatusEventAsync(messageId: string, status: MessageStatus, error: string) {
        if (this.hubConnection.state == HubConnectionState.Connected) {
            await this.hubConnection.send('UpdateMessageStatusAsync', messageId, status, error);
        }
    }

    /**
     * send the command to persist the connection id for this client
     * @param clientId the id of the client app
     */
    public async sendPersistClientConnectionEvent$(clientId: string): Promise<void> {
        if (this.hubConnection.state == HubConnectionState.Connected) {
            await this.hubConnection.send('PersistClientConnectionAsync', clientId, true);
        }
    }

    /**
     * register an event handler to handle send message requests
     * @param handler the handler to be executed when the event is triggered
     */
    public onSendMessageEvent(handler: (message: IMessages) => void): void {
        if (this.hubConnection) {
            this.hubConnection.on('sendSmsMessage', handler);
        }
    }

    /**
    * register an event handler to handle the client info updated request
    * @param handler the handler to be executed when the event is triggered
    */
    public onClientInfoUpdatedEvent(handler: (clientInfo: IAppIdentification) => void): void {
        if (this.hubConnection) {
            this.hubConnection.on('updateClientInfo', handler);
        }
    }

    /**
    * register an event handler to handle disconnection event
    * @param handler the handler to be executed when the event is triggered
    */
    public onForceDisconnectionEvent(handler: (reason: DisconnectionReason) => void): void {
        if (this.hubConnection) {
            this.hubConnection.on('forceDisconnect', handler);
        }
    }

    /**
    * register an event handler to handle client connected event
    * @param handler the handler to be executed when the event is triggered
    */
    public onClientConnectedEvent(handler: () => void): void {
        if (this.hubConnection) {
            this.hubConnection.on('clientConnected', handler);
        }
    }
}
