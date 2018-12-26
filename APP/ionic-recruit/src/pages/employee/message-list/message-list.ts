import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';

@Component({
  selector: 'page-message-list',
  templateUrl: 'message-list.html'
})
export class MessageListPage {
  data: Array<any> = [{},{}];
  constructor(public navCtrl: NavController) {

  }

}
