import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { MapPageRoutingModule } from './preferences-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IndexPage } from './pages/index/index.page';
import { TranslocoModule, TRANSLOCO_SCOPE } from '@ngneat/transloco';

@NgModule({
  imports: [
    IonicModule,
    FormsModule,
    CommonModule,
    TranslocoModule,
    ReactiveFormsModule,
    MapPageRoutingModule
  ],
  declarations: [
    IndexPage,
  ],
  providers: [
    { provide: TRANSLOCO_SCOPE, useValue: 'common', multi: true },
    { provide: TRANSLOCO_SCOPE, useValue: 'preferences', multi: true },
  ]
})
export class PreferencesModule { }
