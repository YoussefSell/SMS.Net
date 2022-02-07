import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContactDetailPageRoutingModule } from './contact-detail-routing.module';
import { ContactDetailPage } from './contact-detail';
import { IonicModule } from '@ionic/angular';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    ContactDetailPageRoutingModule
  ],
  declarations: [
    ContactDetailPage,
  ]
})
export class ContactDetailModule { }
