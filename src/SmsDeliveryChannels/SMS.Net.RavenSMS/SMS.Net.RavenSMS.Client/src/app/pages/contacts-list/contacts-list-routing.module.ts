import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ContactsListPage } from './contacts-list';
const routes: Routes = [
  {
    path: '',
    component: ContactsListPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactsListPageRoutingModule { }
