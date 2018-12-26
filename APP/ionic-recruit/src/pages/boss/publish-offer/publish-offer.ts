import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import {
  edit_offer_detail,
  go_back_offer_manage_tab,
  search_work_types,
  user_info_boss
} from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { AlertService } from "../../../providers/alert.service";
import { ChooseWorkTypePage } from "../../choose-work-type/choose-work-type";
import { BossService } from "../boss-service";
import { OfferDescriptionPage } from "./offer-description/offer-description";
import { Login } from "../../login/login";
import {BossTabsPage} from "../tabs/tabs";

@Component({
  selector: 'page-publish-offer',
  templateUrl: 'publish-offer.html'
})
export class PublishOfferPage {
  id: string = '';
  types: Array<any> = [];
  offerTitle: string = '';
  userInfo: any;
  submitted: boolean = false;
  cities: Array<any> = [];
  selectedCity: string = '';
  hourSalary: number = null;
  weekWorkHours: number = null;
  limitSex: string = '';
  limitLowAge: number = null;
  limitHighAge: number = null;
  degrees: Array<any> = [];
  selectedDegree: string = '';
  requestCount: number = null;
  fee: number = null;
  selectedWorkYear: string = '';
  workYears: Array<any> = [];
  offerDetail: any;
  workTypeSubscription: any;
  offerDescriptionSubscription: any;
  editFlag: boolean = false;
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService,
    private _BossService: BossService,
    private _NavParams: NavParams
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {

    if (this._StorageService.readSession(edit_offer_detail)) {
      this.userInfo = this._StorageService.read(user_info_boss);
      this.id = this._StorageService.readSession(edit_offer_detail);
      this._StorageService.removeSession(edit_offer_detail);
      this.initRequirement();
    }

    // 订阅期望职位
    this.workTypeSubscription = this._ParameterPassingService.expectWorkTypeSource$.subscribe(
      (data: any) => {
        this.types = data;
      });

    this.offerDescriptionSubscription = this._ParameterPassingService.offerDescriptionSource$.subscribe(
      (data: any) => {
        this.offerDetail = data;
      });
  }

  ionViewDidEnter() {
    if (!this._StorageService.read(user_info_boss)) {
      this.navCtrl.push(Login);
    } else {
      this.userInfo = this._StorageService.read(user_info_boss);
      this.getPositionTree();
      this.getWorkCities();
      this.getCultural();
      this.getWorkYears();
    }
  }

  // 保存
  save() {
    this.submitted = true;

    // 非空效验
    if (this.types.length == 0 || this.offerTitle == '' || this.selectedCity == '' || this.hourSalary ==  null || this.weekWorkHours == null || this.limitSex == '' || this.limitLowAge == null || this.limitHighAge == null || this.selectedDegree == '' || this.requestCount == null || !this.offerDetail || this.selectedWorkYear == ''){
      return;
    }

    let tempType = [];
    this.types.forEach(item => {
      tempType.push(item.id);
    });

    let postBody: any = {
      Id: this.id,
      UserId: this.userInfo.Id,
      PK_App_Position_Name: tempType.join(','),
      Title: this.offerTitle,
      PK_App_Country_Name: this.selectedCity,
      PreTaxSalary: this.hourSalary,
      WorkHourPerWeek: this.weekWorkHours,
      WorkLimitSex: this.limitSex,
      WorkLimitAgeLow: this.limitLowAge,
      WorkLimitAgeHigh: this.limitHighAge,
      EnumWorkLimitDegree: this.selectedDegree,
      TotalHire: this.requestCount,
      Tag: this.offerDetail.tags.join(','),
      PublishUserId: this.userInfo.Id,
      Description: this.offerDetail.description,
      SwitchBtnOpen: "0", // 0是关闭,
      EnumWorkPermit: this.selectedWorkYear
    };

    if (this.id != '') {
      this._BossService.editRequirement(postBody).subscribe((data: any) => {
        this._AlertService.presentAlert(data.Message);
        if (data.Code == 200) {
          this.resetInit();
          this._StorageService.writeSession(go_back_offer_manage_tab, true);
          this.navCtrl.push(BossTabsPage);
        }
      });
    } else {
      this._BossService.createRequirement(postBody).subscribe((data: any) => {
        this._AlertService.presentAlert(data.Message);
        if (data.Code == 200) {
          this.resetInit();
          this._StorageService.writeSession(go_back_offer_manage_tab, true);
          this.navCtrl.push(BossTabsPage);
        }
      });
    }
  }

  // 工种
  getPositionTree() {
    this._BossService.getPositionTree(this.userInfo ? this.userInfo.Id : '', false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this._StorageService.writeSession(search_work_types, data.Data);
      }
    });
  }

  // 选择期望职位
  goChooseWorkType() {
    this.navCtrl.push(ChooseWorkTypePage, {
      types: this.types
    });
  }

  // 获取国家
  getWorkCities() {
    this._BossService.getCountries(false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
          this.cities = data.Data;
      }
    });
  }

  // 文化水平
  getCultural() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_CustomerWorkmate.Cultural'
    };
    this._BossService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.degrees = data.Data;
      }
    });
  }

  // 文化水平
  getWorkYears() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_Requirement.EnumWorkPermit'
    };
    this._BossService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.workYears = data.Data;
      }
    });
  }

  goOfferDescription() {
    let tempArray = [];
    if (this.offerDetail) {
      tempArray = [this.offerDetail]
    }
    this.navCtrl.push(OfferDescriptionPage, {
      offerDetail: tempArray
    });
  }

  // 初始化编辑参数
  initRequirement() {
    this._BossService.editRequirementInit(this.userInfo.Id, this.id).subscribe((data: any) => {
      if (data.Code == 200) {
        if (data.Data.PK_App_Position_Name != null && data.Data.PK_App_Position_Name != '') {
          let tempArray = data.Data.PK_App_Position_Name.split(',');
          let tempNameArray = data.Data.App_Position_Name.split(',');
          tempArray.forEach((item, index) => {
            this.types.push({
              name: tempNameArray[index],
              id: item
            });
          });
        }

        if (data.Data.Title != null && data.Data.Title != '') {
          this.offerTitle = data.Data.Title;
        }

        if (data.Data.PK_App_Country_Name != null && data.Data.PK_App_Country_Name != '') {
          this.selectedCity = data.Data.PK_App_Country_Name;
        }

        if (data.Data.PreTaxSalary != null && data.Data.PreTaxSalary != '') {
          this.hourSalary = Number(data.Data.PreTaxSalary);
        }

        if (data.Data.WorkHourPerWeek != null && data.Data.WorkHourPerWeek != '') {
          this.weekWorkHours = Number(data.Data.WorkHourPerWeek);
        }

        if (data.Data.WorkLimitSex != null && data.Data.WorkLimitSex != '') {
          this.limitSex = data.Data.WorkLimitSex;
        }

        if (data.Data.WorkLimitAgeLow != null && data.Data.WorkLimitAgeLow != '') {
          this.limitLowAge = Number(data.Data.WorkLimitAgeLow);
        }

        if (data.Data.WorkLimitAgeHigh != null && data.Data.WorkLimitAgeHigh != '') {
          this.limitHighAge = Number(data.Data.WorkLimitAgeHigh);
        }

        if (data.Data.EnumWorkLimitDegree != null && data.Data.EnumWorkLimitDegree != '') {
          this.selectedDegree = data.Data.EnumWorkLimitDegree;
        }

        if (data.Data.TotalHire != null && data.Data.TotalHire != '') {
          this.requestCount = Number(data.Data.TotalHire);
        }

        if (data.Data.EnumWorkPermit != null && data.Data.EnumWorkPermit != '') {
          this.selectedWorkYear = data.Data.EnumWorkPermit;
        }

        this.offerDetail = {
          tags: (data.Data.Tag != '' && data.Data.Tag != null ? data.Data.Tag.split(',') : []),
          description: (data.Data.Description != '' && data.Data.Description != null ? data.Data.Description : '')
        };
      }
    });
  }

  resetInit() {
    this._ParameterPassingService.setRefreshOption(true);
    this.types = [];
    this.offerTitle = '';
    this.selectedCity = '';
    this.hourSalary = null;
    this.weekWorkHours = null;
    this.limitSex = '';
    this.limitLowAge = null;
    this.limitHighAge = null;
    this.selectedDegree = '';
    this.requestCount = null;
    this.offerDetail = null;
    this.selectedWorkYear = '';
    this.submitted = false;
  }
}
