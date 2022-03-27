import { OverlayEventDetail } from '@ionic/core';
import { AlertController } from '@ionic/angular';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ServerAlertService {

    _alert: HTMLIonAlertElement | null = null;
    _presented = false;

    constructor(
        private _alertController: AlertController
    ) { }

    /**
     * Returns a promise that resolves when the alert did dismiss.
     * @returns a Promise that resolve to @see OverlayEventDetail<any>
     */
    async onDidDismissAsync(expectedRole: string, callback: () => void): Promise<void> {
        if (this._alert) {
            // get the dismiss result
            const result = await this._alert.onDidDismiss();

            // if the role is as expected execute the callback
            if (result.role === expectedRole) {
                callback();
            }
        }
    }

    /**
     * set the alert message text
     * @param message the new message to set for the alert
     */
    async setMessageAsync(message: string): Promise<void> {
        // enure that the alert has been initialized
        await this.ensureAlertInitAsync();

        // set the message
        this._alert.message = message;
    }

    /**
     * set the alert button
     * @param text the button text
     * @param role the button role
     */
    async setButton(text: string, role: string): Promise<void> {
        // enure that the alert has been initialized
        await this.ensureAlertInitAsync();

        // set the button
        this._alert.buttons = [{ text, role }];
    }

    /**
     * show the alert
     * @returns a promise that resolve to void
     */
    async present(): Promise<void> {
        // enure that the alert has been initialized
        await this.ensureAlertInitAsync();

        // if the alert is already presented no need to present
        if (this._presented) {
            return;
        }

        // if the alert is null, there is noting to present
        if (this._alert == null) {
            return;
        }

        await this._alert.present();
    }

    /**
     * hide the alert
     * @returns a promise that resolve to void
     */
    async dismiss(): Promise<void> {
        if (this._alert) {
            await this._alert.dismiss();
            this._alert = null;
            await this.initAlertAsync();
        }
    }

    /**
     * ensure that the alert has been initialized
     */
    private async ensureAlertInitAsync() {
        if (this._alert) {
            return;
        }

        // create the alert instance
        await this.initAlertAsync();
    }

    /**
     * initialize the alert instance
     */
    private async initAlertAsync() {
        this._alert = await this._alertController.create({ backdropDismiss: false });
        this._presented = false;
    }
}
