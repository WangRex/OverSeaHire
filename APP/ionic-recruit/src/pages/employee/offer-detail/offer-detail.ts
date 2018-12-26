import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { DomSanitizer } from "@angular/platform-browser";
import { ApplyOfferPage } from "../apply-offer/apply-offer";
import { AttentionsPage } from "../attentions/attentions";
import { EmployeeService } from "../employee-service";
import { AlertService } from "../../../providers/alert.service";
import { StorageService } from "../../../providers/storage-service";
import {selected_offer, user_info} from "../../../providers/constants";
import { Login } from "../../login/login";

@Component({
  selector: 'page-offer-detail',
  templateUrl: 'offer-detail.html'
})
export class OfferDetailPage {
  userInfo: any;
  id: string = '';
  offerDetail: any;
  constructor(
    public navCtrl: NavController,
    private _DomSanitizer: DomSanitizer,
    private _NavParams: NavParams,
    private _EmployeeService: EmployeeService,
    private _AlertService: AlertService,
    private _StorageService: StorageService
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.id = this._NavParams.get('id');
    this.getRequirementDetail();
  }

  ionViewDidEnter() {
    this.userInfo = this._StorageService.read(user_info);
  }

  assembleHTML(strHTML: any) {
    if(strHTML != '' && strHTML != null) {
      return this._DomSanitizer.bypassSecurityTrustHtml(strHTML);
    } else {
      return '';
    }
  }

  goApplyOfferPage() {
    if(!this.userInfo) {
      this.navCtrl.push(Login);
    } else {
      this._StorageService.writeSession(selected_offer, this.offerDetail);
      this.navCtrl.push(ApplyOfferPage, {
        reqId: this.offerDetail.Id
      });
    }
  }

  goAttentionsPage() {
    this.navCtrl.push(AttentionsPage);
  }

  // 获取需求详情
  getRequirementDetail() {
    this.userInfo = this._StorageService.read(user_info);
    let userID = '';
    if (this.userInfo) {
      userID = this.userInfo.Id;
    }
    this._EmployeeService.getRequirementDetail(this.id, userID).subscribe((data: any) => {
      if (data.Code == 200) {
        this.offerDetail = data.Data;
        this.offerDetail.serviceMoneyTotal = Number(data.Data.ServiceMoney) + Number(data.Data.ServiceTailMoney);
        this.offerDetail.requireDetail = this.offerDetail.WorkLimitSex + '/' + this.offerDetail.WorkLimitAgeLow + '-' + this.offerDetail.WorkLimitAgeHigh + '岁' + '/' + this.offerDetail.WorkLimitDegree;
      }
    });
  }

  toggleCollectReqiurement() {
    if(!this.userInfo) {
      // this._AlertService.presentToast(user_no_login_msg);
      this.navCtrl.push(Login);
    } else {
      // 收藏
      if (this.offerDetail.RequirementCollId == '') {
        this._EmployeeService.collectRequirement(this.userInfo.Id, this.id).subscribe((data: any) => {
          this._AlertService.presentAlert(data.Message);
          if (data.Code == 200) {
              this.offerDetail.RequirementCollId = data.Data;
          }
        });
      } else {
        //取消收藏
        this._EmployeeService.unCollectRequirement(this.userInfo.Id, this.offerDetail.RequirementCollId).subscribe((data: any) => {
          this._AlertService.presentAlert(data.Message);
          if (data.Code == 200) {
            this.offerDetail.RequirementCollId = '';
          }
        });
      }
    }
  }

  // 分享
  share() {
    this._AlertService.presentToast('需要等微信开放平台审核过账号');
  }
}
