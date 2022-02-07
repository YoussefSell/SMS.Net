import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ContactDetailPage } from './contact-detail';

const routes: Routes = [
  {
    path: '',
    component: ContactDetailPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactDetailPageRoutingModule { }
