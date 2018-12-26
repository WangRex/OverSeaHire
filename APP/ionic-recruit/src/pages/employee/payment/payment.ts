import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { StorageService } from "../../../providers/storage-service";
import {selected_offer, user_info} from "../../../providers/constants";
import { AlertService } from "../../../providers/alert.service";
import { NavigationPage } from "../navigation/navigation";
import { EmployeeService } from "../employee-service";
import { Geolocation } from '@ionic-native/geolocation';
import { Utils } from "../../../providers/utils";

@Component({
  selector: 'page-payment',
  templateUrl: 'payment.html'
})
export class PaymentPage {
  label: string = '';
  offerDetail: any;
  wechatPay: boolean = true;
  alipay: boolean = false;
  offlinePay: boolean = false;
  jobId: string = '';
  promisePayWay: string = '';
  servicePayWay: string = '';
  applyStatus: string = '2';
  tailPayWay: string = '';
  longtitude: any = '';
  lattitude: any = '';
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _AlertService: AlertService,
    private _NavParams: NavParams,
    private _EmployeeService: EmployeeService,
    private geolocation: Geolocation,
    private _Utils: Utils
  ) {}

  ionViewDidLoad() {
    this.jobId = this._NavParams.get('jobId');
    this.offerDetail = this._StorageService.readSession(selected_offer);
    this.label = '待支付保证金: ' + this.offerDetail.PromiseMoney + '元';
    if (this.offerDetail.CurrentStep == '5') {
      this.label = '待支付服务费: ' + this.offerDetail.ServiceMoney + '元';
      this.applyStatus = '5';
    }
    if (this.offerDetail.CurrentStep == '8') {
      this.applyStatus = '8';
      this.label = '待支付尾款: ' + this.offerDetail.ServiceTailMoney + '元';
    }
  }

  switchPayMethod(type) {
    if (type == 'wechat') {
      this.wechatPay = true;
      this.alipay = false;
      this.offlinePay = false;
    }

    if (type == 'alipay') {
      this.wechatPay = false;
      this.alipay = true;
      this.offlinePay = false;
    }

    if (type == 'offline') {
      this.wechatPay = false;
      this.alipay = false;
      this.offlinePay = true;
    }
  }

  payment() {
    if (this.offlinePay) {
      this.promisePayWay = '2';
      this.servicePayWay = '';
      this.tailPayWay = '';
      if (this.offerDetail.CurrentStep == '5') {
        this.promisePayWay = '';
        this.servicePayWay = '2';
        this.tailPayWay = '';
      }
      if (this.offerDetail.CurrentStep == '8') {
        this.promisePayWay = '';
        this.servicePayWay = '';
        this.tailPayWay = '2';
      }
      if (!this._Utils.isMobile()) {
        this.editApplyJob();
      } else {
        this.geolocation.getCurrentPosition().then((data) => {
          this.lattitude = data.coords.latitude;
          this.longtitude = data.coords.longitude;
          this.editApplyJob();
        }).catch((error) => {
          this.editApplyJob();
        });
      }
    } else {
      this._AlertService.presentToast('需要开通支付资质');
    }
  }

  // 0 微信支付 1 是支付宝支付 2是线下支付
  editApplyJob() {
    let userInfo: any = this._StorageService.read(user_info);
    let postBody: any = {
      UserId: userInfo.Id,
      ApplyJobId: this.jobId,
      CurrentStep: this.applyStatus,
      EnumPromisePayWay: this.promisePayWay,
      EnumServicePayWay: this.servicePayWay,
      EnumTailPayWay: this.tailPayWay,
      Longitude: this.longtitude,
      Latitude: this.lattitude

    };
    this._EmployeeService.editApplyJob(postBody).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        if (this.offlinePay) {
          this.navCtrl.push(NavigationPage, {
            shop: data.Data.officeVm
          });
        } else {
          // 需要支付资质开通后调整
        }
      }
    });
  }
}
