import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { PaymentPage } from "../payment/payment";
import { EmployeeService } from "../employee-service";
import { Clipboard } from '@ionic-native/clipboard';
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-apply-complete',
  templateUrl: 'apply-complete.html'
})
export class ApplyCompletePage {
  showContactDetail: boolean = false;
  jobId: string = '';
  configDetail = {
    ContactName: "",
    ContactPhone: "",
    ContactWeChat: ""
  };
  constructor(
    public navCtrl: NavController,
    private _EmployeeService: EmployeeService,
    private _Clipboard: Clipboard,
    private _AlertService: AlertService,
    private _NavParams: NavParams
  ) {}

  ionViewDidLoad() {
    this.jobId = this._NavParams.get('jobId');
    this.getSysConfig();
  }

  toggleContactDetail() {
    this.showContactDetail  = !this.showContactDetail;
  }

  goPaymentPage() {
    this.showContactDetail = false;
    this.navCtrl.push(PaymentPage, {
      jobId: this.jobId
    });
  }

  // 获取咨询信息
  getSysConfig() {
    this._EmployeeService.getSysConfiguration().subscribe((data: any) => {
      if (data.Code == 200) {
        this.configDetail = data.Data;
      }
    });
  }

  copyWechat() {
    this._Clipboard.copy(this.configDetail.ContactWeChat);
    this._AlertService.presentToast('复制成功');
  }
}
