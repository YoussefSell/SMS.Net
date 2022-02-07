import { Tabs } from './tabs-page';
import { NgModule } from '@angular/core';
import { MessagesPage } from '../messages/messages';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'tabs',
    component: Tabs,
    children: [
      {
        path: 'messages',
        children: [
          {
            path: '',
            component: MessagesPage,
          },
          {
            path: ':messageId',
            loadChildren: () => import('../message-detail/message-detail.module').then(m => m.MessageDetailModule)
          }
        ]
      },
      {
        path: 'contacts',
        children: [
          {
            path: '',
            loadChildren: () => import('../contacts-list/contacts-list.module').then(m => m.ContactsListModule)
          },
          {
            path: 'contacts-details/:contactId',
            loadChildren: () => import('../contact-detail/contact-detail.module').then(m => m.ContactDetailModule)
          }
        ]
      },
      {
        path: 'preferences',
        children: [
          {
            path: '',
            loadChildren: () => import('../preferences/preferences.module').then(m => m.PreferencesModule)
          }
        ]
      },
      {
        path: 'about',
        children: [
          {
            path: '',
            loadChildren: () => import('../about/about.module').then(m => m.AboutModule)
          }
        ]
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

