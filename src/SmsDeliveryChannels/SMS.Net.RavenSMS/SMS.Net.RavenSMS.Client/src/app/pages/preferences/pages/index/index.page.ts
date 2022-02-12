import { RootStoreState, UIStoreActions } from 'src/app/store';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';
import { SettingsStoreSelectors } from 'src/app/store/settings-store';
import { IAppIdentification, IServerInfo } from 'src/app/core/models';

@Component({
  selector: 'page-preferences-index',
  templateUrl: 'index.page.html',
  styleUrls: ['index.page.scss']
})
export class IndexPage implements OnInit {

  subsink = new SubSink();
  settingsForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private store: Store<RootStoreState.State>,
  ) { }

  ngOnInit(): void {
    this.settingsForm = this.fb
      .group({
        darkMode: this.fb.control(false),
        clientId: this.fb.control(''),
        serverURL: this.fb.control(''),
        clientName: this.fb.control(''),
        serverStatus: this.fb.control(''),
        clientDescription: this.fb.control(''),
      });

    this.subsink.sink = this.settingsForm.get('darkMode').valueChanges
      .subscribe(value => {
        this.store.dispatch(UIStoreActions.updateDarkMode({ value }))
      });

    this.subsink.sink = this.store.select(SettingsStoreSelectors.StateSelector)
      .subscribe(state => {
        this.initializeForm(state.appIdentification, state.serverInfo);
      })
  }

  initializeForm(appIdentification: IAppIdentification, serverInfo: IServerInfo): void {
    this.settingsForm.get('clientId').setValue(appIdentification.clientId);
    this.settingsForm.get('clientName').setValue(appIdentification.clientName);
    this.settingsForm.get('clientDescription').setValue(appIdentification.clientDescription);
    this.settingsForm.get('serverURL').setValue(serverInfo.serverUrl);
    this.settingsForm.get('serverStatus').setValue(serverInfo.status);
  }
}
