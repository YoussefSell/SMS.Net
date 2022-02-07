import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MessagesPage } from './messages';

const routes: Routes = [
  {
    path: '',
    component: MessagesPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MessagesPageRoutingModule { }
