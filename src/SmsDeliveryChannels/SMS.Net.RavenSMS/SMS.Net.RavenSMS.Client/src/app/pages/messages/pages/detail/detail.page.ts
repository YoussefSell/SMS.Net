import { Component } from '@angular/core';

import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'page-message-detail',
  styleUrls: ['./detail.page.scss'],
  templateUrl: 'detail.page.html'
})
export class DetailPage {
  session: any;
  isFavorite = false;
  defaultHref = '';

  constructor(
    private route: ActivatedRoute
  ) { }

  ionViewWillEnter() {

  }

  ionViewDidEnter() {
    this.defaultHref = `/app/tabs/messages`;
  }

  sessionClick(item: string) {
  }

  toggleFavorite() {

  }

  shareSession() {
  }
}
