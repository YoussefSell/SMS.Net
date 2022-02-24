import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { Tabs } from './tabs-page';
import { TabsPageRoutingModule } from './tabs-routing.module';

import { AboutModule } from '../about/about.module';
import { MessagesModule } from '../messages/messages.module';
import { PreferencesModule } from '../preferences/preferences.module';

@NgModule({
  imports: [
    AboutModule,
    CommonModule,
    IonicModule,
    PreferencesModule,
    MessagesModule,
    TabsPageRoutingModule
  ],
  declarations: [
    Tabs,
  ]
})
export class TabsModule { }
