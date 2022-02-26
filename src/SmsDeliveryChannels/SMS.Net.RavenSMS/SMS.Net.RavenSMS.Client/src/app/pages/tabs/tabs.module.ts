import { TranslocoModule, TRANSLOCO_SCOPE } from '@ngneat/transloco';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';

import { PreferencesModule } from '../preferences/preferences.module';
import { TabsPageRoutingModule } from './tabs-routing.module';
import { MessagesModule } from '../messages/messages.module';
import { AboutModule } from '../about/about.module';
import { Tabs } from './tabs-page';


@NgModule({
  imports: [
    AboutModule,
    CommonModule,
    IonicModule,
    PreferencesModule,
    MessagesModule,
    TranslocoModule,
    TabsPageRoutingModule
  ],
  declarations: [
    Tabs,
  ],
  providers: [
    { provide: TRANSLOCO_SCOPE, useValue: 'common', multi: true },
    { provide: TRANSLOCO_SCOPE, useValue: 'tabs', multi: true },
  ]
})
export class TabsModule { }
