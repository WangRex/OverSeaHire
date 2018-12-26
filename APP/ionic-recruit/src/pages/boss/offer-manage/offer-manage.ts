import {Component, ViewChild } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { Content } from 'ionic-angular';
import { BossOfferDetailPage } from "../offer-detail/offer-detail";
import { user_info_boss } from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { BossService } from "../boss-service";
import { Login } from "../../login/login";
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-offer-manage',
  templateUrl: 'offer-manage.html'
})
export class OfferManagePage {
  @ViewChild(Content) content: Content;
  type: string = '';
  workerId: string = '';
  userInfo: any;
  loadMore: boolean = false;
  pageNum: number = 1;
  data: Array<any> = [];
  searched: boolean = false;

  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _BossService: BossService,
    private _NavParams: NavParams,
    private _AlertService: AlertService
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {}

  ionViewDidEnter() {
    if (!this._StorageService.read(user_info_boss)) {
      this.navCtrl.push(Login);
    } else {
      if (this._NavParams.get('type')) {
        this.type = this._NavParams.get('type');
        this.workerId = this._NavParams.get('workerId');
      }
      this.userInfo = this._StorageService.read(user_info_boss);
      this.getMyOffers();
    }
  }

  goOfferDetailPage(id) {
    this.navCtrl.push(BossOfferDetailPage, {
      id: id
    });
  }

  getMyOffers(event?) {
    this._BossService.getRequirementList(this.userInfo.Id, this.pageNum).subscribe((data: any) => {
      if (data.Code == 200) {
        this.searched = true;
        data.Data.requirementVms.forEach(item => {
          if (item.SwitchBtnOpen == '1') {
            item.status = '开放中';
          } else {
            item.status = '已关闭'
          }
        });
        if (this.pageNum == 1) {
          this.data = data.Data.requirementVms;
        } else {
          this.data = this.data.concat(data.Data.requirementVms);
        }
        this.loadMore = this.data.length < data.DataCount ? true : false;
      }

      if (event) {
        event.complete();
      }
    });
  }

  // 下拉刷新
  doInfinite(infiniteScroll) {
    this.pageNum++;
    this.getMyOffers(infiniteScroll);
  }

  // 上拉刷新
  doRefresh(refresher?) {
    this.pageNum = 1;
    this.getMyOffers(refresher);
  }

  // 选中职位进行判断
  chooseOffer(id) {
    if (this.type == 'offer-invite') {
      this.sendOfferInvite(id);
    } else {
      this.goOfferDetailPage(id);
    }
  }

  // 发送邀请
  sendOfferInvite(offerId) {
    this._BossService.inviteWorker(this.userInfo.Id, offerId, this.workerId).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        this.navCtrl.popToRoot();
      }
    });
  }
}
