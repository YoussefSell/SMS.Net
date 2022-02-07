import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'page-map',
  templateUrl: 'preferences.html',
  styleUrls: ['./preferences.scss']
})
export class PreferencesPage {
  // Our local settings object
  options: any;

  dark = false;

  profileSettings = {
    page: 'profile',
    pageTitleKey: 'SETTINGS_PAGE_PROFILE'
  };

  page: string = 'main';
  pageTitleKey: string = 'SETTINGS_TITLE';
  pageTitle: string;

  subSettings: any = PreferencesPage;
  constructor() { }


}
