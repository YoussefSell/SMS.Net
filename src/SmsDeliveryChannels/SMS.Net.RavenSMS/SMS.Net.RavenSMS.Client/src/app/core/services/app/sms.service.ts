import { SMS } from '@awesome-cordova-plugins/sms/ngx';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SmsService {
    constructor(private sms: SMS) { }

    /**
     * send the sms message to the given number
     * @param phoneNumber the phone number to send the message to
     * @param message the sms message content
     */
    async sendSmsAsync(phoneNumber: string, message: string): Promise<void> {
        await this.sms.send(phoneNumber, message, {
            android: { intent: '' }
        });
    }
}

