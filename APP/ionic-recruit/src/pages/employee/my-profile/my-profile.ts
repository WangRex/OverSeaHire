import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { MyWorkerPage } from "../my-worker/my-worker";
import { PersonalInfoPage } from "../personal-info/personal-info";
import { ExpectOfferPage } from "../expect-offer/expect-offer";
import { MyFavoriteOfferPage } from "../my-favorite-offer/my-favorite-offer";
import { StorageService } from "../../../providers/storage-service";
import { user_info, file_url } from "../../../providers/constants";
import { Login } from "../../login/login";
import { SettingPage } from "../../setting/setting";
// import { EmployeeService } from "../employee-service";
import { AlertService } from "../../../providers/alert.service";
import { LoginService } from "../../login/login-service";
import { user_no_login_msg } from "../../../providers/constants";
import { MyAppliedOfferPage } from "../my-applied-offer/my-applied-offer";
import { MyInterviewOfferPage } from "../my-interview-offer/my-interview-offer";
import { MyRecommendOfferPage } from "../my-recommend-offer/my-recommend-offer";
import { FeedbackPage } from "../../feedback/feedback";

@Component({
  selector: 'page-my-profile',
  templateUrl: 'my-profile.html'
})
export class MyProfilePage {
  fileUrl = file_url;
  userInfo: any;
  customerInfo: any;
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    // private _EmployeeService: EmployeeService,
    private _AlertService: AlertService,
    private _LoginService: LoginService,
  ) {}

  ionViewDidEnter() {
    if (this._StorageService.read(user_info)) {
      this.userInfo  = this._StorageService.read(user_info);
      this.login(this.userInfo.Phone);
    }

  }

  goMyWorkerPage() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(MyWorkerPage, {
        previous: 'myProfile'
      });
    }
  }

  goPersonalInfoPage() {
    this.navCtrl.push(PersonalInfoPage);
  }

  goExpectOfferPage() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(ExpectOfferPage);
    }
  }

  goMyFavoritePage() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(MyFavoriteOfferPage);
    }
  }

  goMyAppliedPage() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(MyAppliedOfferPage);
    }
  }

  goInterviewPage() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(MyInterviewOfferPage);
    }
  }

  goRecommendPage() {
    if (!this.userInfo) {
      this._AlertService.presentAlert(user_no_login_msg);
    } else {
      this.navCtrl.push(MyRecommendOfferPage);
    }
  }

  goLoginPage() {
    this.navCtrl.push(Login);
  }

  // getCustomerInfo(id) {
  //   this._EmployeeService.getCustomerInfo(id).subscribe((data: any) => {
  //     if (data.Code == 200) {
  //      this.customerInfo = data.Data;
  //     }
  //   });
  // }

  login(phone) {
      this._LoginService.login(phone, '8888', '0', false).subscribe((data: any) => {
        if (data.Code == 200) {
          this._StorageService.write(user_info, data.Data);
          this.userInfo  = this._StorageService.read(user_info);
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
}
