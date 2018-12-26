import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';

@Component({
  selector: 'page-boss-message-list',
  templateUrl: 'message-list.html'
})
export class BossMessageListPage {
  data: Array<any> = [{},{}];
  constructor(public navCtrl: NavController) {

  }

}
