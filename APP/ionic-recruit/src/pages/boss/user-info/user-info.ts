import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { user_info_boss } from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { BossService } from "../boss-service";
import { InAppBrowser } from '@ionic-native/in-app-browser';
import { AlertService } from "../../../providers/alert.service";
import { Clipboard } from '@ionic-native/clipboard';
import { OfferManagePage } from "../offer-manage/offer-manage";

@Component({
  selector: 'page-user-info',
  templateUrl: 'user-info.html'
})
export class UserInfoPage {
  configDetail = {
    ContactName: "",
    ContactPhone: "",
    ContactWeChat: ""
  };
  userInfo: any;
  workerId: string = '';
  workerDetail: any;
  videos: Array<any> = [];
  showContactDetail: boolean = false;
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _BossService: BossService,
    private _NavParams: NavParams,
    private _InAppBrowser: InAppBrowser,
    private _AlertService: AlertService,
    private _Clipboard: Clipboard
  ) {
    this.userInfo = this._StorageService.read(user_info_boss);
  }

  // 首次加载时候执行
  ionViewDidLoad() {
    this.workerId = this._NavParams.get('workerId');
    this.getUserDetail();
    this.getSysConfig();
  }

  ionViewWillLeave() {
    this.showContactDetail = false;
  }

  // 获取工友详情
  getUserDetail() {
    this._BossService.getCustomerWorkmate(this.userInfo.Id, this.workerId).subscribe((data: any) => {
      if (data.Code == 200) {
        this.workerDetail = data.Data;
        this.workerDetail.description = this.workerDetail.Sex + '/' + this.workerDetail.Age + '/' + this.workerDetail.DriverLicenceName + '/' + this.workerDetail.JobIntensionNames + '/' + this.workerDetail.BirthPlace;
        if (this.workerDetail.AbroadExp != '' && this.workerDetail.AbroadExp != null) {
          this.workerDetail.description += '/' + this.workerDetail.AbroadExpName;
        }
        if (this.workerDetail.VideoPath != '' && this.workerDetail.VideoPath != null) {
          if (this.workerDetail.VideoPath.indexOf('\r\n') != -1) {
            this.videos = this.workerDetail.VideoPath.split('\r\n');
          } else {
            this.videos = this.workerDetail.VideoPath.split('\n');
          }
        }
      }
    });
  }

  // 用浏览器打开视频地址
  previewVideo(url) {
    this._InAppBrowser.create(url, '_blank');
  }

  // 显示或者隐藏咨询内容
  toggleContactDetail() {
    this.showContactDetail  = !this.showContactDetail;
  }

  // 获取咨询信息
  getSysConfig() {
    this._BossService.getSysConfiguration().subscribe((data: any) => {
      if (data.Code == 200) {
        this.configDetail = data.Data;
      }
    });
  }

  // 复制微信号
  copyWechat() {
    this._Clipboard.copy(this.configDetail.ContactWeChat);
    this._AlertService.presentToast('复制成功');
  }

  // 跳转到职位列表
  goOfferListPage() {
    this.navCtrl.push(OfferManagePage, {
      type: 'offer-invite',
      workerId: this.workerDetail.Id
    });
  }

  // 收藏用户
  collectUser() {
    this._BossService.collectCustomer(this.userInfo.Id, this.workerId).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        this.workerDetail.CustomerCollectId = data.Data;
      }
    });
  }

  unCollectUser() {
    this._BossService.unCollectCustomer(this.userInfo.Id, this.workerDetail.CustomerCollectId).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        this.workerDetail.CustomerCollectId = '';
      }
    });
  }
}
