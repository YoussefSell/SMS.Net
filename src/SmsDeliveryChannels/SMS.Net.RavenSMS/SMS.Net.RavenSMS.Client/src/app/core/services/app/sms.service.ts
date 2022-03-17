import { SMS } from '@awesome-cordova-plugins/sms/ngx';
import { Capacitor } from '@capacitor/core';
import { Injectable } from '@angular/core';
import { IResult } from '../../models';

@Injectable({ providedIn: 'root' })
export class SmsService {

    private sms: SMS;
    private platform: string;
    private hasPermission: boolean;

    constructor() {
        // there is an issue with DI i couldn't 
        // inject the SMS instance into the constructor
        this.sms = new SMS();

        // get the platform
        this.platform = Capacitor.getPlatform();
    }

    /**
     * check if the app has permission to send sms messages.
     * @returns returns a promise that resolves with a boolean that indicates if we have permission
     */
    HasPermissionAsync(): Promise<boolean> {
        return this.sms.hasPermission();
    }

    /**
     * send the sms message to the given number
     * @param phoneNumber the phone number to send the message to
     * @param message the sms message content
     */
    async sendSmsAsync(phoneNumber: string, message: string): Promise<IResult> {
        if (this.platform != 'web') {
            return this.sendSmsNativeAsync(phoneNumber, message);
        }

        // for the web we will assume the message has been sent.
        return { isSuccess: true }
    }

    async sendSmsNativeAsync(phoneNumber: string, message: string): Promise<IResult> {
        try {
            var result = await this.sms.send(phoneNumber, message, { android: { intent: '' } });

            // check if the sending has succeeded
            if (result === 'OK') {
                return { isSuccess: true };
            }

            // the sending has failed
            return {
                isSuccess: false,
                error: 'sending_failed_unknown'
            }
        } catch (error) {
            return {
                isSuccess: false,
                error: 'sending_failed_error'
            };
        }
    }
}
