import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { EmployeeTabsPage } from "../employee/tabs/tabs";
import { BossTabsPage } from "../boss/tabs/tabs";
import { boss_flag } from "../../providers/constants";
import { StorageService } from "../../providers/storage-service";

@Component({
  selector: 'page-choose-role',
  templateUrl: 'choose-role.html'
})
export class ChooseRolePage {

  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService
  ) {}

  goWorkerHomePage() {
    this._StorageService.writeSession(boss_flag, false);
    this.navCtrl.push(EmployeeTabsPage)
  }

  goBossHomePage() {
    this._StorageService.writeSession(boss_flag, true);
    this.navCtrl.push(BossTabsPage);
  }

}
