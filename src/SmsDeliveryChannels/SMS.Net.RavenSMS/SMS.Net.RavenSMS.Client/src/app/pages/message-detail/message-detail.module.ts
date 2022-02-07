import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MessageDetailPageRoutingModule } from './message-detail-routing.module';
import { MessageDetailPage } from './message-detail';
import { IonicModule } from '@ionic/angular';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    MessageDetailPageRoutingModule
  ],
  declarations: [
    MessageDetailPage,
  ]
})
export class MessageDetailModule { }
