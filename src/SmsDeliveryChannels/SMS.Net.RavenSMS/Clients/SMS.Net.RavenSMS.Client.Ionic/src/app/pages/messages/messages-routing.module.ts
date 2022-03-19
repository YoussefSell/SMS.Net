import { DetailPage } from './pages/detail/detail.page';
import { RouterModule, Routes } from '@angular/router';
import { IndexPage } from './pages/index/index.page';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: IndexPage
  },
  {
    path: ':messageId',
    component: DetailPage
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MessagesPageRoutingModule { }
