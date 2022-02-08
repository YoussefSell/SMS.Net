import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
    providedIn: 'root'
})
export class SignalRService {

    private hubConnection: HubConnection

    public initConnection = () => {

        this.hubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7114/RavenSMS/Hub')
            .build();

        this.hubConnection
            .start()
            .then(() => console.log('Connection started'))
            .catch(err => console.log('Error while starting connection: ' + err))
    }

    public addTransferDataListener = () => {
        this.hubConnection.on('transferchartdata', (data) => {
            console.log('on message sent', data);
        });
    }
}