import { Component } from '@angular/core';

import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'page-message-detail',
  styleUrls: ['./message-detail.scss'],
  templateUrl: 'message-detail.html'
})
export class MessageDetailPage {
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
    console.log('Clicked', item);
  }

  toggleFavorite() {

  }

  shareSession() {
    console.log('Clicked share session');
  }
}
