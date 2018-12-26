import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { OrderDetailPage } from "../order-detail/order-detail";
import { EmployeeService } from "../employee-service";
import {user_info, job_steps, selected_offer} from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { Utils } from "../../../providers/utils";
import { Geolocation } from '@ionic-native/geolocation';
import {NavigationPage} from "../navigation/navigation";
import { PaymentPage } from "../payment/payment";

@Component({
  selector: 'page-order-list',
  templateUrl: 'order-list.html'
})
export class OrderListPage {
  showImg: boolean = true;
  userInfo: any;
  loadMore: boolean = false;
  pageNum: number = 1;
  data: Array<any> = [];
  steps: Array<any> = [];
  constructor(
    public navCtrl: NavController,
    private _EmployeeService: EmployeeService,
    private _StorageService: StorageService,
    private _Utils: Utils,
    private geolocation: Geolocation,
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.getApplyStep();
  }

  ionViewDidEnter() {
    this.userInfo = this._StorageService.read(user_info);
    this.pageNum = 1;
    this.getApplyJobs();
  }

  goOrderDetail(id) {
    this.navCtrl.push(OrderDetailPage, {
      id: id
    });
  }

  // 获取申请流程
  getApplyStep() {
    this._EmployeeService.getApplyStep().subscribe((data: any) => {
      if (data.Code == 200) {
        this.steps = data.Data;
        this._StorageService.writeSession(job_steps, data.Data);
      }
    });
  }

  // 获取申请流程
  getApplyJobs(event?) {
    this._EmployeeService.getApplyJobs(this.userInfo ? this.userInfo.Id : '', this.pageNum).subscribe((data: any) => {
      if (data.Code == 200) {
        data.Data.forEach(item => {
          item.requireDetail = item.requirementDetailVm.WorkLimitSex + '/' + item.requirementDetailVm.WorkLimitAgeLow + '-' + item.requirementDetailVm.WorkLimitAgeHigh + '岁' + '/' + item.requirementDetailVm.WorkLimitDegree;
          item.userDes = item.applyJobUserVm.Sex + '/' + item.applyJobUserVm.Age + '/' + item.applyJobUserVm.BirthPlace;
          if (item.applyJobUserVm.AbroadExp != null && item.applyJobUserVm.AbroadExp != '') {
            item.userDes += '/' + item.applyJobUserVm.AbroadExp;
          }
        });
        if (this.pageNum == 1) {
          this.data = (data.Data == null ? [] : data.Data);
        } else {
          this.data = this.data.concat((data.Data == null ? [] : data.Data));
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
    this.getApplyJobs(infiniteScroll);
  }

  // 上拉刷新
  doRefresh(refresher?) {
    this.pageNum = 1;
    this.getApplyJobs(refresher);
  }

  // 导航到代理点
  goNavigationPage(applyId) {
    if (!this._Utils.isMobile()) {
      this.getJobDetail(applyId);
    } else {
      this.geolocation.getCurrentPosition().then((data) => {
        this.getJobDetail(applyId, data.coords.latitude, data.coords.longitude);
      }).catch((error) => {
        this.getJobDetail(applyId);
      });
    }
  }

  // 获取申请详情
  getJobDetail(applyId, latitude: any = '', longitude: any = '') {
    this._EmployeeService.getApplyJob(this.userInfo.Id, applyId, latitude, longitude).subscribe((data: any) => {
      if (data.Code == 200) {
        this.navCtrl.push(NavigationPage, {
          shop: data.Data.officeVm
        });
      }
    });
  }

  payment(option) {
    let orderDetail = {
      CurrentStep: option.CurrentStep,
      PromiseMoney: option.PromiseMoney,
      ApplyJobPromiseMoney: option.ApplyJobPromiseMoney,
      ServiceMoney: option.ServiceMoney,
      ServiceTailMoney: option.ServiceTailMoney
    };
    this._StorageService.writeSession(selected_offer, orderDetail);
    this.navCtrl.push(PaymentPage, {
      jobId: option.ApplyJobId
    });
  }

}
