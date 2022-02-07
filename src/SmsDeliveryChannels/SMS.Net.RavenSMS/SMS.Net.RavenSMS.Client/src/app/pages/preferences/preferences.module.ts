import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { PreferencesPage } from './preferences';
import { MapPageRoutingModule } from './preferences-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    MapPageRoutingModule
  ],
  declarations: [
    PreferencesPage,
  ]
})
export class PreferencesModule { }
