import { Component } from '@angular/core';
import { HomePage } from "../home/home";
import { OrderListPage } from "../order-list/order-list";
import { MessageListPage } from "../message-list/message-list";
import { MyProfilePage } from "../my-profile/my-profile";

@Component({
  templateUrl: 'tabs.html'
})
export class EmployeeTabsPage {

  tab1Root = HomePage;
  tab2Root = OrderListPage;
  tab3Root = MessageListPage;
  tab4Root = MyProfilePage;

  constructor() {

  }
}
