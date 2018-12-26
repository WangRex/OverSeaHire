import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { StorageService } from "../../../providers/storage-service";
import {user_info_boss, file_url, user_no_login_msg} from "../../../providers/constants";
import { Login } from "../../login/login";
import { AlertService } from "../../../providers/alert.service";
import { LoginService } from "../../login/login-service";
import { SettingPage } from "../../setting/setting";
import { FeedbackPage } from "../../feedback/feedback";
import { CompanyAuthPage } from "../company-auth/company-auth";

@Component({
  selector: 'page-my-profile-boss',
  templateUrl: 'my-profile.html'
})
export class MyProfileBossPage {
  fileUrl = file_url;
  userInfo: any;
  customerInfo: any;
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _AlertService: AlertService,
    private _LoginService: LoginService,
  ) {}

  ionViewDidEnter() {
    if (this._StorageService.read(user_info_boss)) {
      this.userInfo  = this._StorageService.read(user_info_boss);
      this.login(this.userInfo.Phone);
    }

  }

  goLoginPage() {
    this.navCtrl.push(Login);
  }


  login(phone) {
      this._LoginService.login(phone, '8888', '1', false).subscribe((data: any) => {
        if (data.Code == 200) {
          this._StorageService.write(user_info_boss, data.Data);
          this.userInfo  = this._StorageService.read(user_info_boss);
        }
      });
  }

  goSettingPage() {
    // if (!this.userInfo) {
    //   this._AlertService.presentAlert(user_no_login_msg);
    // } else {
      this.navCtrl.push(SettingPage);
    // }
  }

  goFeedback() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(FeedbackPage);
    }
  }

  goCompayAuth() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(CompanyAuthPage);
    }
  }

}
