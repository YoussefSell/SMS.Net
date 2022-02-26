import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { IndexPage } from './pages/index/index.page';

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
export class AboutPageRoutingModule { }
