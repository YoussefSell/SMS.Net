import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';

import { MessagesPageRoutingModule } from './messages-routing.module';

import { IndexPage } from './pages/index/index.page';
import { DetailPage } from './pages/detail/detail.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    MessagesPageRoutingModule
  ],
  declarations: [
    IndexPage,
    DetailPage,
  ]
})
export class MessagesModule { }
