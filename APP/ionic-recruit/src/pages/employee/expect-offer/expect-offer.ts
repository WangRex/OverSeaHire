import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { EmployeeService } from "../employee-service";
import {search_work_types, user_info} from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
// import { ChooseWorkTypePage } from "../choose-work-type/choose-work-type";
import { ChooseWorkTypePage } from "../../choose-work-type/choose-work-type";
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { ChooseCityPage } from "../choose-city/choose-city";
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-expect-offer',
  templateUrl: 'expect-offer.html'
})
export class ExpectOfferPage {

  id: string = '';
  submitted: boolean = false;
  submittedPlus: boolean = false;
  showFixedContent: boolean = false;
  // 添加证书
  showAddCertificateContent: boolean = false;
  startDate: any = null;
  endDate: any = null;
  company: string = '';
  certificate: string = '';
  cers: Array<any> = [];

  selectedSalary: any = {
    ItemValue: ''
  };
  selectedLan: any = {
    ItemValue: ''
  };
  selectedExp: any = {
    ItemValue: ''
  };

  userInfo: any;
  types: Array<any> = [];
  cities: Array<any> = [];
  salaries: Array<any> = [];
  skills: Array<any> = [];
  languages: Array<any> = [];
  exps: Array<any> = [];
  workTypeSubscription: any;
  workCitySubscription: any;
  constructor(
    public navCtrl: NavController,
    private _EmployeeService: EmployeeService,
    private _StorageService: StorageService,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService
  ) {
    this.userInfo = this._StorageService.read(user_info);
  }

  // 首次加载时候执行
  ionViewDidLoad() {
    this.getCustomerIntension();
    this.getSalaries();
    this.getWorkSkill();
    this.getLanguage();
    this.getOutsiteExp();
    this.getPositionTree();

    // 订阅期望职位
    this.workTypeSubscription = this._ParameterPassingService.expectWorkTypeSource$.subscribe(
      (data: any) => {
        this.types = data;
      });

    // 订阅期望国家
    this.workCitySubscription = this._ParameterPassingService.expectCitySource$.subscribe(
      (data: any) => {
        this.cities = data;
      });
  }

  getSalaries() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App.EnumSalary'
    };
    this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
       this.salaries = data.Data;
        this.salaries.forEach(item => {
          if (item.ItemValue == this.selectedSalary.ItemValue) {
            this.selectedSalary = item;
          }
        });
      }
    });
  }

  // 选择期望职位
  goChooseWorkType() {
    this.navCtrl.push(ChooseWorkTypePage, {
      types: this.types
    });
  }

  // 选择期望职位
  goChooseWorkCity() {
    this.navCtrl.push(ChooseCityPage, {
      types: this.cities
    });
  }

  // 掌握技能
  getWorkSkill() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_CustomerJobIntension.Skills'
    };
    this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.skills = data.Data;
      }
    });
  }

  // 外语等级
  getLanguage() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_CustomerJobIntension.EnumForeignLangGrade'
    };
    this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.languages = data.Data;
        this.languages.forEach(item => {
          if (item.ItemValue == this.selectedLan.ItemValue) {
            this.selectedLan = item;
          }
        });
      }
    });
  }

  // 出国经历
  getOutsiteExp() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_CustomerJobIntension.AbroadExp'
    };
    this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.exps = data.Data;
        data.Data.forEach(item => {
          if(item.ItemValue == this.selectedExp.ItemValue) {
            this.selectedExp = item;
          }
        });
      }
    });
  }

  // 点击证书
  addCertificate() {
    this.showAddCertificateContent = true;
    this.showFixedContent = true;
  }

  // 保存证书
  saveCertificate() {
    this.submittedPlus = true;
    if (this.startDate != null && this.endDate != null && this.company.trim() != '' && this.certificate != '') {
      this.cers.push({
        startDate: this.startDate,
        endDate: this.endDate,
        company: this.company,
        certificate: this.certificate
      });
      this.startDate = null;
      this.endDate = null;
      this.company =  '';
      this.certificate = '';
      this.showAddCertificateContent = false;
      this.showFixedContent = false;
      this.submittedPlus = false;
    }
  }

  // date: yyyy-MM
  formatDateDisplay(date) {
    let tempArray = date.split('-');
    return tempArray[0] + '年' + tempArray[1] + '月';
  }

  // 保存求职意向
  save() {
    this.submitted = true;

    // 非空效验
    if (this.types.length == 0 || this.selectedSalary.ItemValue == '' || this.cities.length ==  0 || this.selectedLan.ItemValue == '' || this.selectedExp.ItemValue == ''){
      return;
    }

    let tempType = [];
    this.types.forEach(item => {
      tempType.push(item.id);
    });

    let tempCity = [];
    this.cities.forEach(item => {
      tempCity.push(item.Id);
    });


    let temp = [];
    this.cers.forEach(item => {
      temp.push({
        PK_App_Customer_CustomerName: this.userInfo.Id,
        StartDate: item.startDate,
        EndDate: item.endDate,
        Name: item.certificate,
        Company: item.company
      });
    });

    let postBody: any = {
      Id: this.id,
      PK_App_Customer_CustomerName: this.userInfo.Id,
      EnumPositionType: tempType.join(','),
      ExpectSalary: this.selectedSalary.ItemValue,
      ExpectCountry: tempCity.join(','),
      EnumForeignLangGrade: this.selectedLan.ItemValue,
      AbroadExp: this.selectedExp.ItemValue,
      certificatePosts: temp,

    };
    this._EmployeeService.updateCustomerJobInt(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        this.userInfo.IntensionFlag = true;
        this._StorageService.write(user_info, this.userInfo);
        this.submitted = false;
        this._AlertService.presentAlert(data.Message);
        this.navCtrl.pop();
      } else {
        this._AlertService.presentAlert(data.Message);
      }
    });
  }

// 获取求职意向
  getCustomerIntension() {
    this._EmployeeService.getCustomerIntension(this.userInfo.Id).subscribe((data: any) => {
      if (data.Code == 200) {
        // 初始化参数
        this.id = data.Data.Id;
        if (data.Data.AbroadExp != null && data.Data.AbroadExp != '') {
          this.selectedExp.ItemValue = data.Data.AbroadExp;
          this.exps.forEach(item => {
            if (item.ItemValue == this.selectedExp.ItemValue) {
              this.selectedExp = item;
            }
          });
        }

        if (data.Data.EnumForeignLangGrade != null && data.Data.EnumForeignLangGrade != '') {
          this.selectedLan.ItemValue = data.Data.EnumForeignLangGrade;
          this.languages.forEach(item => {
            if (item.ItemValue == this.selectedLan.ItemValue) {
              this.selectedLan = item;
            }
          });
        }

        if (data.Data.ExpectSalary != null && data.Data.ExpectSalary != '') {
          this.selectedSalary.ItemValue = data.Data.ExpectSalary;
          this.salaries.forEach(item => {
            if (item.ItemValue == this.selectedSalary.ItemValue) {
              this.selectedSalary = item;
            }
          });
        }

        data.Data.certificatePosts.forEach(item => {
          this.cers.push({
            startDate: item.StartDate,
            endDate: item.EndDate,
            company: item.Company,
            certificate: item.Name
          });
        });

        if (data.Data.EnumPositionType != null && data.Data.EnumPositionType != '') {
          let tempArray = data.Data.EnumPositionType.split(',');
          let tempNameArray = data.Data.PositionNames.split(',');
          tempArray.forEach((item, index) => {
            this.types.push({
              name: tempNameArray[index],
              id: item
            });
          });
        }

        if (data.Data.ExpectCountry != null && data.Data.ExpectCountry != '') {
          let tempArray = data.Data.ExpectCountry.split(',');
          tempArray.forEach(item => {
            this.cities.push({
              Id: item,
            });
          });
        }

      } else {
        this._AlertService.presentToast(data.Message);
      }
    });
  }

  // 工种
  getPositionTree() {
    this._EmployeeService.getPositionTree(this.userInfo.Id, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this._StorageService.writeSession(search_work_types, data.Data);
      }
    });
  }
}
