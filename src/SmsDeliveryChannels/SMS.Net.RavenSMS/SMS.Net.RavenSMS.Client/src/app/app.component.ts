import { RootStoreState, StorePersistenceActions, UIStoreSelectors } from './store';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService } from './core/services';
import { App } from '@capacitor/app';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-root',
  template: `
  <ion-app [class.dark-theme]="dark">
    <ion-router-outlet id="main-content"></ion-router-outlet>
  </ion-app>
  `,
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {

  subSink = new SubSink();
  dark: boolean = false;

  constructor(
    public signalRService: SignalRService,
    private store: Store<RootStoreState.State>,
  ) {
    // add a listener on the app state
    App.addListener('appStateChange', ({ isActive }) => {
      if (!isActive) {
        this.store.dispatch(StorePersistenceActions.PersistStore());
      }
    });
  }

  ngOnInit(): void {
    this.subSink.sink = this.store.select(UIStoreSelectors.IsDarkModeSelector)
      .subscribe(value => {
        this.dark = value;
      });

    this.signalRService.initConnection();
  }

  ngOnDestroy(): void {
    this.subSink.unsubscribe();
  }
}
