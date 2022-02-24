import { Component } from '@angular/core';

@Component({
  template: `
  <ion-tabs>
    <ion-tab-bar slot="bottom">
      <ion-tab-button tab="messages">
        <ion-icon name="mail"></ion-icon>
        <ion-label>Messages</ion-label>
      </ion-tab-button>
      <ion-tab-button tab="contacts">
        <ion-icon name="people"></ion-icon>
        <ion-label>Contacts</ion-label>
      </ion-tab-button>
      <ion-tab-button tab="preferences">
        <ion-icon name="settings"></ion-icon>
        <ion-label>Preferences</ion-label>
      </ion-tab-button>
      <ion-tab-button tab="about">
        <ion-icon name="information-circle"></ion-icon>
        <ion-label>About</ion-label>
      </ion-tab-button>
    </ion-tab-bar>
  </ion-tabs>
  `,
  styles: [
    '.tabbar {justify-content: center;}',
    '.tab-button {max-width: 200px;}'
  ]
})
export class Tabs { }
