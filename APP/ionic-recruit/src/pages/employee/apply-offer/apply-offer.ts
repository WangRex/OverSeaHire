import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { MyWorkerPage } from "../my-worker/my-worker";
import { user_info } from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { EmployeeService } from "../employee-service";
import { AlertService } from "../../../providers/alert.service";
import { ApplyCompletePage } from "../apply-complete/apply-complete";
import { PersonalInfoPage } from "../personal-info/personal-info";

@Component({
  selector: 'page-apply-offer',
  templateUrl: 'apply-offer.html'
})
export class ApplyOfferPage {
  reqId: string = '';
  userInfo: any;
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _EmployeeService: EmployeeService,
    private _AlertService: AlertService,
    private _NavParams: NavParams
  ) {}

  ionViewDidLoad() {
    this.reqId = this._NavParams.get('reqId');
  }

  ionViewDidEnter() {
    this.userInfo = this._StorageService.read(user_info);
  }

  // 给自己报名
  goMyPersonalPage() {
    if (this.userInfo.IntroFlag) {
      let postBody = {
        UserId: this.userInfo.Id,
        RequirementId: this.reqId,
        CustomerId: this.userInfo.Id
      };
      this._EmployeeService.createApplyJob(postBody).subscribe((data: any) => {
        this._AlertService.presentToast(data.Message);
        if (data.Code == 200) {
          this.navCtrl.push(ApplyCompletePage, {
            jobId: data.Data
          });
        }
      });
    } else {
      this.navCtrl.push(PersonalInfoPage);
    }
  }

  goMyWorkerPage() {
    this.navCtrl.push(MyWorkerPage, {
      reqId: this.reqId
    });
  }
}
