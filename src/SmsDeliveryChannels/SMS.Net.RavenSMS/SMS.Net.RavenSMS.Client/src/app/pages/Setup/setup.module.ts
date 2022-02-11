import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';

import { SetupRoutingModule } from './setup-routing.module';
import { IndexPage } from './pages/index/index.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SetupRoutingModule
  ],
  declarations: [
    IndexPage
  ],
  bootstrap: [IndexPage],
})
export class SetupModule { }
