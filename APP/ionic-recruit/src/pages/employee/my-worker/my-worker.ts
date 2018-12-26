import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ApplyCompletePage } from "../apply-complete/apply-complete";
import { AddWorkerPage } from "../add-worker/add-worker";
import { EmployeeService } from "../employee-service";
import { user_info, selected_offer } from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-my-worker',
  templateUrl: 'my-worker.html'
})
export class MyWorkerPage {
  data: Array<any> = [];
  loadMore: boolean = false;
  pageNum: number = 1;
  previousPage = 'offer';
  constructor(
    public navCtrl: NavController,
    private _EmployeeService: EmployeeService,
    private _StorageService: StorageService,
    private _NavParams: NavParams,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService
  ) {}

  ionViewDidLoad() {
    this.previousPage = this._NavParams.get('previous');
  }

  ionViewDidEnter() {
    this.getMyWorker();
  }

  chooseWorker(option) {
    if (this.previousPage == 'myProfile') {
      this.navCtrl.push(AddWorkerPage, {
        id: option.Id
      });
    } else {
      this.applyOfferForWorker(option);
    }
  }

  // 添加工友
  goAddWorkerPage() {
    this.navCtrl.push(AddWorkerPage);
  }

  getMyWorker(event?) {
    let userInfo: any  = this._StorageService.read(user_info);
    this._EmployeeService.getMyWorkerList(userInfo.Id, this.pageNum).subscribe((data: any) => {
      if (data.Code == 200) {
        data.Data.forEach(item => {
          item.description = item.Sex + '/' + item.Age + '/' + item.Cultural + '/' + item.DriverLicenceName + '/' + item.JobIntensionNames + '/' + item.BirthPlace;
          if (item.AbroadExp != '' && item.AbroadExp != null) {
            item.description += '/' + item.AbroadExpName;
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
    this.getMyWorker(infiniteScroll);
  }

  // 上拉刷新
  doRefresh(refresher?) {
    this.pageNum = 1;
    this.getMyWorker(refresher);
  }

  // 给工友报名
  applyOfferForWorker(option) {
    let userInfo: any  = this._StorageService.read(user_info);
    let offerDetail: any = this._StorageService.readSession(selected_offer);
    let postBody = {
      UserId: userInfo.Id,
      RequirementId: offerDetail.Id,
      CustomerId: option.Id
    };
    this._EmployeeService.createApplyJob(postBody).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200) {
        this.navCtrl.push(ApplyCompletePage);
      }
    });
  }
}
