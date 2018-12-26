import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import {file_url, job_steps, user_info_boss} from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { AlertService } from "../../../providers/alert.service";
import { BossService } from "../boss-service";

@Component({
  selector: 'page-interview-detail',
  templateUrl: 'interview-detail.html'
})
export class InterviewDetailPage {
  orderDetail: any;
  jobId: string = '';
  fileUrl = file_url;
  currentStep: any;
  steps: Array<any> = [];
  customerName: string;
  customerPhoto: any;
  customerDes: string;
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _NavParams: NavParams,
    private _BossService: BossService,
    private _AlertService: AlertService
  ) {}

  ionViewDidLoad() {
    this.steps = this._StorageService.readSession(job_steps);
    this.jobId = this._NavParams.get('jobId');
    this.customerDes = this._NavParams.get('description');
    this.customerName = this._NavParams.get('name');
    this.customerPhoto = this._NavParams.get('photo');
    this.getJobDetail();
  }

  ionViewDidEnter() {}

  // 获取订单详情
  getJobDetail() {
    let userInfo: any  = this._StorageService.read(user_info_boss);
    this._BossService.getApplyJob(userInfo.Id, this.jobId).subscribe((data: any) => {
      if (data.Code == 200) {
        this.orderDetail = data.Data;
        this.orderDetail.userDes = this.orderDetail.applyJobUserVm.Sex + '/' + this.orderDetail.applyJobUserVm.Age + '/' + this.orderDetail.applyJobUserVm.BirthPlace;
        if (this.orderDetail.applyJobUserVm.AbroadExp != null && this.orderDetail.applyJobUserVm.AbroadExp != '') {
          this.orderDetail.userDes += '/' + this.orderDetail.applyJobUserVm.AbroadExp;
        }
        this.currentStep = Number(this.orderDetail.CurrentStep);
      }
    });
  }

  rejectInterviewStatus() {
    let userInfo: any  = this._StorageService.read(user_info_boss);
    let postBody = {
      UserId: userInfo.Id,
      ApplyJobId: this.jobId,
      CurrentStep: this.orderDetail.CurrentStep,
      Result: '未通过'
    };
    this._BossService.editApplyJob(postBody).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        this.navCtrl.popToRoot();
      }
    });
  }


}
