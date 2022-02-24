import { Tabs } from './tabs-page';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'tabs',
    component: Tabs,
    children: [
      {
        path: 'messages',
        loadChildren: () => import('../messages/messages.module').then(m => m.MessagesModule)
      },
      {
        path: 'preferences',
        loadChildren: () => import('../preferences/preferences.module').then(m => m.PreferencesModule)
      },
      {
        path: 'about',
        loadChildren: () => import('../about/about.module').then(m => m.AboutModule)
      },
      {
        path: '',
        redirectTo: '/app/tabs/messages',
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TabsPageRoutingModule { }

