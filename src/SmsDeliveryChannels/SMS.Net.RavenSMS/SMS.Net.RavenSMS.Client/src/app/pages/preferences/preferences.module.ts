import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { MapPageRoutingModule } from './preferences-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IndexPage } from './pages/index/index.page';

@NgModule({
  imports: [
    IonicModule,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    MapPageRoutingModule
  ],
  declarations: [
    IndexPage,
  ]
})
export class PreferencesModule { }
