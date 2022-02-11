import { BarcodeScanner } from '@capacitor-community/barcode-scanner';
import { RootStoreState, UIStoreActions } from 'src/app/store';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'page-setup-index',
  templateUrl: 'index.page.html',
  styleUrls: ['index.page.scss'],
})
export class IndexPage {

  subsink = new SubSink();
  settingsForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private store: Store<RootStoreState.State>,
  ) { }

  ngOnInit(): void {

  }

}
