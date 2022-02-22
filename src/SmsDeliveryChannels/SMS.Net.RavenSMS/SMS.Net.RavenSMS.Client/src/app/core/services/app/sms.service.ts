// import { SMS } from '@awesome-cordova-plugins/sms/ngx';
import { Injectable } from '@angular/core';
declare var SMS: any;

@Injectable({ providedIn: 'root' })
export class SmsService {
    constructor() { }

    /**
     * send the sms message to the given number
     * @param phoneNumber the phone number to send the message to
     * @param message the sms message content
     */
    async sendSmsAsync(phoneNumber: string, message: string): Promise<void> {
        await SMS.send(phoneNumber, message, {
            android: { intent: '' }
        });
    }
}

