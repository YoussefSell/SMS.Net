import { StatePersistenceReducer, StorePersistenceEffects } from './store/state-persistence';
import { RouteReuseStrategy, RouterModule } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { IonicStorageModule } from '@ionic/storage-angular';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { BrowserModule } from '@angular/platform-browser';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { environment } from 'src/environments/environment';
import { RootEffects } from './store/root-effects';
import { RootStoreModule } from './store';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    IonicStorageModule.forRoot(),

    RootStoreModule,
    StoreModule.forRoot({}, { metaReducers: StatePersistenceReducer.metaReducers }),
    EffectsModule.forRoot([RootEffects, StorePersistenceEffects.StorePersistenceEffects]),
    StoreDevtoolsModule.instrument({ maxAge: 25, name: 'Sms.Net - RavenSMS', logOnly: environment.production }),

    RouterModule.forRoot([
      {
        path: '',
        redirectTo: '/app/tabs/messages',
        pathMatch: 'full'
      },
      {
        path: 'app',
        loadChildren: () => import('./pages/tabs/tabs.module').then(m => m.TabsModule)
      }
    ])
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
  ],
  bootstrap: [
    AppComponent
  ],
})
export class AppModule { }
