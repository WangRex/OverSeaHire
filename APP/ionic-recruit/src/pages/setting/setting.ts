import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { StorageService } from "../../providers/storage-service";
import { ChooseRolePage } from "../choose-role/choose-role";
import { AlertService } from "../../providers/alert.service";

@Component({
  selector: 'page-setting',
  templateUrl: 'setting.html'
})
export class SettingPage {

  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _AlertService: AlertService
  ) {

  }

  logout() {
    this._StorageService.clear();
    this._StorageService.clearSession();
    this.navCtrl.push(ChooseRolePage);
  }

  promptMsg() {
    this._AlertService.presentToast('请提供具体内容和样式');
  }
}
