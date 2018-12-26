import {Component, ViewChild} from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { EmployeeService } from "../employee-service";
import { StorageService } from "../../../providers/storage-service";
import {job_steps, user_info, file_url, selected_offer} from "../../../providers/constants";
import { DomSanitizer } from "@angular/platform-browser";
import { CurrentOrderStepsPage } from "../current-order-steps/current-order-steps";
import { NavigationPage } from "../navigation/navigation";
import { AlertService } from "../../../providers/alert.service";
import {PaymentPage} from "../payment/payment";
import { Content } from "ionic-angular";

@Component({
  selector: 'page-order-detail',
  templateUrl: 'order-detail.html'
})
export class OrderDetailPage {
  @ViewChild(Content) content: Content;
  fileUrl = file_url;
  steps: Array<any> = [];
  userInfo: any;
  id: string = '';
  orderDetail: any;
  paymentLabel: string = '';
  currentStep: number;
  showInfo: boolean = false;
  markers: Array<any> = [];
  constructor(
    public navCtrl: NavController,
    private _EmployeeService: EmployeeService,
    private _StorageService: StorageService,
    private _NavParams: NavParams,
    private _DomSanitizer: DomSanitizer,
    private _AlertService: AlertService
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.id = this._NavParams.get('id');
    this.userInfo = this._StorageService.read(user_info);
    this.steps = this._StorageService.readSession(job_steps);
    this.getJobDetail();
  }

  // 获取申请流程
  getJobDetail() {
    this._EmployeeService.getApplyJob(this.userInfo.Id, this.id).subscribe((data: any) => {
      if (data.Code == 200) {
        this.markers = [data.Data.officeVm];
        this.orderDetail = data.Data;
        this.orderDetail.requireDetail = this.orderDetail.requirementDetailVm.WorkLimitSex + '/' + this.orderDetail.requirementDetailVm.WorkLimitAgeLow + '-' + this.orderDetail.requirementDetailVm.WorkLimitAgeHigh + '岁' + '/' + this.orderDetail.requirementDetailVm.WorkLimitDegree;
        this.orderDetail.userDes = this.orderDetail.applyJobUserVm.Sex + '/' + this.orderDetail.applyJobUserVm.Age + '/' + this.orderDetail.applyJobUserVm.BirthPlace;
        if (this.orderDetail.applyJobUserVm.AbroadExp != null && this.orderDetail.applyJobUserVm.AbroadExp != '') {
          this.orderDetail.userDes += '/' + this.orderDetail.applyJobUserVm.AbroadExp;
        }
        if (this.orderDetail.CurrentStep == '2') {
          this.paymentLabel = '待支付保证金：' + this.orderDetail.ApplyJobPromiseMoney + '元';
        }
        if (this.orderDetail.CurrentStep == '5') {
          this.paymentLabel = '待支付服务费：' + this.orderDetail.ServiceMoney + '元';
        }
        if (this.orderDetail.CurrentStep == '8') {
          this.paymentLabel = '待支付尾款：' + this.orderDetail.ServiceTailMoney + '元';
        }
        this.currentStep = Number(this.orderDetail.CurrentStep) - 1;
        this.content.resize();
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

  goStepDes() {
    this.navCtrl.push(CurrentOrderStepsPage, {
      currentStep: this.currentStep,
      completeSteps: this.orderDetail.applyJobRecordVms
    });
  }

  goNavigationPage() {
    this.navCtrl.push(NavigationPage, {
      shop: this.orderDetail.officeVm
    });
  }

  // 取消订单
  editApplyJob() {
    let postBody: any = {
      UserId: this.userInfo.Id,
      ApplyJobId: this.orderDetail.ApplyJobId,
      EnumApplyStatus: '2'

    };
    this._EmployeeService.editApplyJob(postBody).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        this.navCtrl.pop();
      }
    });
  }

  payment() {
    let orderDetail = {
      CurrentStep: this.orderDetail.CurrentStep,
      PromiseMoney: this.orderDetail.PromiseMoney,
      ApplyJobPromiseMoney: this.orderDetail.ApplyJobPromiseMoney,
      ServiceMoney: this.orderDetail.ServiceMoney,
      ServiceTailMoney: this.orderDetail.ServiceTailMoney
    };
    this._StorageService.writeSession(selected_offer, orderDetail);
    this.navCtrl.push(PaymentPage, {
      jobId: this.orderDetail.ApplyJobId
    });
  }
}
