import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SmsService {
    constructor() { }

    /**
     * send the sms message to the given number
     * @param phoneNumber the phone number to send the message to
     * @param message the sms message content
     */
    sendSms$(phoneNumber: string, message: string): void {
        throw new Error('not implemented');
    }
}

