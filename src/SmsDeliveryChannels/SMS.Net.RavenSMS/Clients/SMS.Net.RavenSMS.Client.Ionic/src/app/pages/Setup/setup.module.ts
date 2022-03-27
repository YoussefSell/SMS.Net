import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';

import { SetupRoutingModule } from './setup-routing.module';
import { IndexPage } from './pages/index/index.page';
import { TranslocoModule, TRANSLOCO_SCOPE } from '@ngneat/transloco';

@NgModule({
  imports: [
    IonicModule,
    FormsModule,
    CommonModule,
    TranslocoModule,
    ReactiveFormsModule,
    SetupRoutingModule
  ],
  declarations: [
    IndexPage
  ],
  providers: [
    { provide: TRANSLOCO_SCOPE, useValue: 'common', multi: true },
    { provide: TRANSLOCO_SCOPE, useValue: 'setup', multi: true },
  ],
  bootstrap: [IndexPage],
})
export class SetupModule { }
