import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { ContactsListPage } from './contacts-list';
import { ContactsListPageRoutingModule } from './contacts-list-routing.module';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    ContactsListPageRoutingModule
  ],
  declarations: [ContactsListPage],
})
export class ContactsListModule { }
