import { SMS } from '@awesome-cordova-plugins/sms/ngx';
import { Injectable } from '@angular/core';
import { IResult } from '../../models';

@Injectable({ providedIn: 'root' })
export class SmsService {
    private sms: SMS;

    constructor() {
        // there is an issue with DI i couldn't 
        // inject the SMS instance into the constructor
        this.sms = new SMS();
    }

    /**
     * send the sms message to the given number
     * @param phoneNumber the phone number to send the message to
     * @param message the sms message content
     */
    async sendSmsAsync(phoneNumber: string, message: string): Promise<IResult> {
        try {
            var result = await this.sms.send(phoneNumber, message, {
                android: { intent: '' }
            });

            // check if the sending has succeeded
            if (result === 'OK') {
                return {
                    isSuccess: true,
                };
            }

            // the sending has failed
            return {
                isSuccess: false,
                error: 'sending_failed_unknown'
            }
        } catch (error) {
            console.log('failed error', error);
            return {
                isSuccess: false,
                error: 'sending_failed_error'
            }
        }
    }
}

