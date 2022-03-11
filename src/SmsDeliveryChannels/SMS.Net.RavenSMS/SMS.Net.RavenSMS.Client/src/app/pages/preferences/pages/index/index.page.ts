import { SettingsStoreSelectors } from 'src/app/store/settings-store';
import { IAppIdentification, IServerInfo } from 'src/app/core/models';
import { RootStoreState, UIStoreActions, UIStoreSelectors } from 'src/app/store';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'page-preferences-index',
  templateUrl: 'index.page.html',
  styleUrls: ['index.page.scss']
})
export class IndexPage {

  _subsink = new SubSink();
  _settingsForm: FormGroup;

  _serverInfo: IServerInfo | null = null;
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
      .subscribe(translationObj => {
        this._languages = Object.keys(translationObj)
          .map(key => ({ value: key, label: translationObj[key] }));
      });

    this._subsink.sink = this._settingsForm.get('darkMode').valueChanges
      .subscribe(value => {
        this.store.dispatch(UIStoreActions.updateDarkMode({ value }))
      });

    this._subsink.sink = this._settingsForm.get('language').valueChanges
      .subscribe(value => {
        this.store.dispatch(UIStoreActions.updateLanguage({ value }))
      });

    this._subsink.sink = this.store.select(UIStoreSelectors.StateSelector)
      .subscribe(state => {
        const darkModeControl = this._settingsForm.get('darkMode');
        if (darkModeControl.value !== state.darkMode) {
          darkModeControl.setValue(state.darkMode);
        }

        const languageControl = this._settingsForm.get('language');
        if (languageControl.value !== state.language) {
          languageControl.setValue(state.language);
        }
      });

    this._subsink.sink = this.store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        this._serverInfo = state.serverInfo;
        this._appIdentification = state.appIdentification;
      });
  }

  ionViewDidLeave(): void {
    this._subsink.unsubscribe();
  }
}
