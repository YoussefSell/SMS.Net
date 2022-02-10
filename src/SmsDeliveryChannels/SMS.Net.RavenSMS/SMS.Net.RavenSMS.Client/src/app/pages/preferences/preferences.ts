import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { RootStoreState, UIStoreActions } from 'src/app/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'page-map',
  templateUrl: 'preferences.html',
  styleUrls: ['./preferences.scss']
})
export class PreferencesPage implements OnInit {

  subsink = new SubSink();
  settingsForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private store: Store<RootStoreState.State>,
  ) { }

  ngOnInit(): void {
    // init the form
    this.initializeForm();

    this.subsink.sink = this.settingsForm.get('darkMode').valueChanges
      .subscribe(value => {
        this.store.dispatch(UIStoreActions.updateDarkMode({ value }))
      });
  }

  initializeForm(): void {
    this.settingsForm = this.fb
      .group({
        darkMode: this.fb.control(false),
        clientId: this.fb.control('clt_rhmf7bvhq8is9'),
        clientName: this.fb.control('Default App'),
        clientDescription: this.fb.control('sample client description'),
        serverURL: this.fb.control('https://localhost:7114'),
        serverStatus: this.fb.control('online')
      });
  }
}
