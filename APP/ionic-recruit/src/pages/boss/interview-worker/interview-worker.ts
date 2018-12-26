import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import {job_steps, user_info_boss} from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { InterviewDetailPage } from "../interview-detail/interview-detail";
import { BossService } from "../boss-service";

@Component({
  selector: 'page-interview-worker',
  templateUrl: 'interview-worker.html'
})
export class InterviewWorkerPage {
  data: Array<any> = [];
  loadMore: boolean = false;
  pageNum: number = 1;
  reqId: string = '';
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _NavParams: NavParams,
    private _BossService: BossService,
  ) {}

  ionViewDidLoad() {
    this.reqId = this._NavParams.get('id');
    this.getInterviewWorker();
    if (!this._StorageService.removeSession(job_steps)) {
      this.getApplyStep();
    }
  }

  ionViewDidEnter() {}

  getInterviewWorker(event?) {
    let userInfo: any  = this._StorageService.read(user_info_boss);
    this._BossService.getInterviewUsers(userInfo.Id, this.reqId, this.pageNum).subscribe((data: any) => {
      if (data.Code == 200) {
        data.Data.forEach(item => {
          item.description = item.Sex + '/' + item.Age + '/' + item.DriverLicence + '/' + item.JobIntensionName + '/' + item.BirthPlace;
          if (item.AbroadExp != '' && item.AbroadExp != null) {
            item.description += '/' + item.AbroadExp;
          }
        });
        if (this.pageNum == 1) {
          this.data = data.Data;
        } else {
          this.data = this.data.concat(data.Data);
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
    this.getInterviewWorker(infiniteScroll);
  }

  // 上拉刷新
  doRefresh(refresher?) {
    this.pageNum = 1;
    this.getInterviewWorker(refresher);
  }

  goInterviewDetail(option) {
    this.navCtrl.push(InterviewDetailPage, {
      jobId: option.ApplyJobId,
      name: option.CustomerName,
      photo: option.Photo,
      description: option.description
    });
  }

  // 获取申请流程
  getApplyStep() {
    this._BossService.getApplyStep().subscribe((data: any) => {
      if (data.Code == 200) {
        this._StorageService.writeSession(job_steps, data.Data);
      }
    });
  }
}
