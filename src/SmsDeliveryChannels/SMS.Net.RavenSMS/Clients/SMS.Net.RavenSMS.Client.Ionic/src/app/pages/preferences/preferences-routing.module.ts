import { RouterModule, Routes } from '@angular/router';
import { IndexPage } from './pages/index/index.page';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: IndexPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MapPageRoutingModule { }
