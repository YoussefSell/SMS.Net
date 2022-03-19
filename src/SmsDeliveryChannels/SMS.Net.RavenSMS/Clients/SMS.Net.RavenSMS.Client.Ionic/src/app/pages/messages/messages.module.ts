import { TranslocoModule, TRANSLOCO_SCOPE } from '@ngneat/transloco';
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
    TranslocoModule,
    MessagesPageRoutingModule,
  ],
  declarations: [
    IndexPage,
    DetailPage,
  ],
  providers: [
    { provide: TRANSLOCO_SCOPE, useValue: 'common', multi: true },
    { provide: TRANSLOCO_SCOPE, useValue: 'messages', multi: true },
  ]
})
export class MessagesModule { }
