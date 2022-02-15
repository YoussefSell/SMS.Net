import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
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

    public addTransferDataListener = () => {
        this.hubConnection.on('transferchartdata', (data) => {
            console.log('on message sent', data);
        });
    }
}