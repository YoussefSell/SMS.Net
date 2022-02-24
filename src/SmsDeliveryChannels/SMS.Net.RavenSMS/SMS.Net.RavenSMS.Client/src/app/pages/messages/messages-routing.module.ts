import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailPage } from './pages/detail/detail.page';
import { IndexPage } from './pages/index/index.page';

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
