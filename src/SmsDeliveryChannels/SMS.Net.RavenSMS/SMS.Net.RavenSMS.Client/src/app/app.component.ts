import { RootStoreState, StorePersistenceActions, RootActions, UIStoreSelectors } from './store';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
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
    private store: Store<RootStoreState.State>,
  ) { }

  ngOnInit(): void {
    this.subSink.sink = this.store.select(UIStoreSelectors.IsDarkModeSelector)
      .subscribe(e => {
        this.dark = e; console.log(e);
      });
  }

  ngOnDestroy(): void {
    this.subSink.unsubscribe();
  }
}
