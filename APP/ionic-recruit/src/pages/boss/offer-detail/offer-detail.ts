import { Component } from '@angular/core';
import { NavController, NavParams} from 'ionic-angular';
import {edit_offer_detail, user_info_boss} from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { BossService } from "../boss-service";
import { DomSanitizer } from "@angular/platform-browser";
import { InterviewWorkerPage } from "../interview-worker/interview-worker";
import { AlertService } from "../../../providers/alert.service";
import { PublishOfferPage } from "../publish-offer/publish-offer";
import { BossTabsPage } from "../tabs/tabs";
import {UserInfoPage} from "../user-info/user-info";

@Component({
  selector: 'page-boss-offer-detail',
  templateUrl: 'offer-detail.html'
})
export class BossOfferDetailPage {
  userInfo: any;
  id: string = '';
  offerDetail: any;
  constructor(
    public navCtrl: NavController,
    private _NavParams: NavParams,
    private _StorageService: StorageService,
    private _BossService: BossService,
    private _DomSanitizer: DomSanitizer,
    private _AlertService: AlertService
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.userInfo = this._StorageService.read(user_info_boss);
    this.id = this._NavParams.get('id');
    this.getRequirementDetail();
  }

  ionViewDidEnter() {}

  // 获取需求详情
  getRequirementDetail() {
    this._BossService.getRequirementDetail(this.id, this.userInfo.Id).subscribe((data: any) => {
      if (data.Code == 200) {
        data.Data.RecommendUsers.forEach(item => {
          item.description = item.Sex + '/' + item.Age + '/' + item.DriverLicence + '/' + item.JobIntensionName + '/' + item.BirthPlace;
          if (item.AbroadExp != '' && item.AbroadExp != null) {
            item.description += '/' + item.AbroadExp;
          }
        });
        this.offerDetail = data.Data;
        this.offerDetail.serviceMoneyTotal = Number(data.Data.ServiceMoney) + Number(data.Data.ServiceTailMoney);
        this.offerDetail.requireDetail = this.offerDetail.WorkLimitSex + '/' + this.offerDetail.WorkLimitAgeLow + '-' + this.offerDetail.WorkLimitAgeHigh + '岁' + '/' + this.offerDetail.WorkLimitDegree;
      }
    });
  }

  assembleHTML(strHTML: any) {
    if(strHTML != '' && strHTML != null) {
      return this._DomSanitizer.bypassSecurityTrustHtml(strHTML);
    } else {
      return '';
    }
  }

  // 跳转到面试工友列表
  goInterviewWorkerPage() {
    this.navCtrl.push(InterviewWorkerPage, {
      id: this.offerDetail.Id
    });
  }

  // 删除此岗位
  deleteRequirement() {
    this._AlertService.presentAlertWithCallback('', '确定要删除此岗位吗?').then(yes => {
      if (yes) {
        this._BossService.deleteRequirement(this.userInfo.Id, this.id).subscribe((data: any) => {
          this._AlertService.presentToast(data.Message);
          if (data.Code == 200) {
            this.navCtrl.pop();
          }
        });
      }
    });
  }

  // 关闭此岗位
  closeRequirement() {
    this._AlertService.presentAlertWithCallback('', '确定要关闭此岗位吗?').then(yes => {
      if (yes) {
        let postbody = {
          Id: this.id,
          UserId: this.userInfo.Id,
          SwitchBtnOpen: '0'
        };
        this._BossService.editRequirement(postbody).subscribe((data: any) => {
          this._AlertService.presentToast(data.Message);
          if (data.Code == 200) {
            this.navCtrl.pop();
          }
        });
      }
    })
  }

  // 修改此岗位
  goPublishOfferPage() {
    this._StorageService.writeSession(edit_offer_detail, this.id);
    this.navCtrl.push(BossTabsPage);
  }

  // 跳转到推荐人详情
  goUserDetail(id) {
    this.navCtrl.push(UserInfoPage, {
      workerId: id
    });
  }
}
