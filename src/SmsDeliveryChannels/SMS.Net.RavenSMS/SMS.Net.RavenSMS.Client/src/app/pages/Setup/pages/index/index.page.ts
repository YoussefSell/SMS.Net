import { PopoverController } from '@ionic/angular';
import { Component } from '@angular/core';

@Component({
  selector: 'page-setup-index',
  templateUrl: 'index.page.html',
  styleUrls: ['index.page.scss'],
})
export class IndexPage {

  constructor(public popoverCtrl: PopoverController) { }
}
