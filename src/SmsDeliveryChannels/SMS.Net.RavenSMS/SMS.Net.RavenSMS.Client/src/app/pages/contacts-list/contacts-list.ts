import { Component } from '@angular/core';

@Component({
  selector: 'page-contacts-list',
  templateUrl: 'contacts-list.html',
  styleUrls: ['./contacts-list.scss'],
})
export class ContactsListPage {
  speakers: any[] = [];

  constructor() { }

  ionViewDidEnter() {

  }
}
