import { TranslocoModule, TRANSLOCO_SCOPE } from '@ngneat/transloco';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';

import { AboutPageRoutingModule } from './about-routing.module';
import { IndexPage } from './pages/index/index.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TranslocoModule,
    AboutPageRoutingModule
  ],
  declarations: [IndexPage],
  entryComponents: [],
  bootstrap: [IndexPage],
  providers: [
    { provide: TRANSLOCO_SCOPE, useValue: 'common', multi: true },
    { provide: TRANSLOCO_SCOPE, useValue: 'about', multi: true },
  ]
})
export class AboutModule { }
