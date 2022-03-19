import { SettingsStoreSelectors } from 'src/app/store/settings-store';
import { IAppIdentification, IServerInfo } from 'src/app/core/models';
import { RootStoreSelectors, RootStoreState, UIStoreActions, UIStoreSelectors } from 'src/app/store';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';
import { ServerStatus } from 'src/app/core/constants/enums';
import { State } from 'src/app/store/ui-store/state';

@Component({
  selector: 'page-preferences-index',
  templateUrl: 'index.page.html',
  styleUrls: ['index.page.scss']
})
export class IndexPage {

  _subsink = new SubSink();
  _settingsForm: FormGroup;

  _serverInfo: IServerInfo | null = null;
  _serverStatus: ServerStatus = ServerStatus.UNKNOWN;
  _appIdentification: IAppIdentification | null = null;

  _languages: { value: string; label: string }[] = [];

  constructor(
    private fb: FormBuilder,
    private store: Store<RootStoreState.State>,
    private translationService: TranslocoService,
  ) {
    this._settingsForm = this.fb
      .group({
        darkMode: this.fb.control(false),
        language: this.fb.control(''),
      });
  }

  ionViewDidEnter(): void {
    this._subsink.sink = this.translationService.selectTranslateObject('languages', {}, 'common')
      .subscribe(translationObj => this._languages = Object.keys(translationObj)
        .map(key => ({ value: key, label: translationObj[key] })));

    this._subsink.sink = this._settingsForm.get('darkMode').valueChanges
      .subscribe(value => this.store.dispatch(UIStoreActions.updateDarkMode({ value })));

    this._subsink.sink = this._settingsForm.get('language').valueChanges
      .subscribe(value => this.store.dispatch(UIStoreActions.updateLanguage({ value })));

    this._subsink.sink = this.store.select(UIStoreSelectors.StateSelector)
      .subscribe(state => this.setDarkMode(state));

    this._subsink.sink = this.store.select(RootStoreSelectors.ServerConnectionSelector)
      .subscribe(state => this._serverStatus = state);

    this._subsink.sink = this.store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        this._serverInfo = state.serverInfo;
        this._appIdentification = state.appIdentification;
      });
  }

  ionViewDidLeave(): void {
    this._subsink.unsubscribe();
  }

  private setDarkMode(state: State) {
    const darkModeControl = this._settingsForm.get('darkMode');
    if (darkModeControl.value !== state.darkMode) {
      darkModeControl.setValue(state.darkMode);
    }

    const languageControl = this._settingsForm.get('language');
    if (languageControl.value !== state.language) {
      languageControl.setValue(state.language);
    }
  }
}
