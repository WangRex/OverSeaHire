import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { PersonalIntroPage } from "../personal-intro/personal-intro";
import {file_url, search_work_types, user_info} from "../../../providers/constants";
import {ChooseHomeTownPage} from "../choose-home-town/choose-home-town";
import {ParameterPassingService} from "../../../providers/rxjs-parameter-passing";
import {StorageService} from "../../../providers/storage-service";
import {PersonalVideoPage} from "../personal-video/personal-video";
import { ActionSheetController } from 'ionic-angular';
import { EmployeeService } from "../employee-service";
import { NativeService } from "../../../providers/native-service";
import { ChooseWorkTypePage } from "../../choose-work-type/choose-work-type";
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-add-worker',
  templateUrl: 'add-worker.html'
})
export class AddWorkerPage {
  id: string = '';
  passport: string = '';
  cultural: any = {
    ItemValue: ''
  };
  types: Array<any> = [];
  userInfo: any;
  submitted: boolean = false;
  customerName: string = '';
  customerTel: number = null;
  customerSex: string = '';
  customerAge: number = null;
  customerHeight: number = null;
  fileUrl = file_url;
  headImg: string = '';
  wordFile: any = {
    path: '',
    FileName: '',
    FileExt: '',
  };
  personalIntroSubscription: any;
  personalIntro: string = '';
  videoData: any = {
    path: ''
  };
  personalVideoSubscription: any;
  personalHomeTownSubscription: any;
  hometown: any = {
    value: '',
    displayName: "请填写您的籍贯"
  };

  culs: Array<any> = [];
  skills: Array<any> = [];
  languages: Array<any> = [];
  exps: Array<any> = [];
  selectedSalary: string = '';
  selectedLan: any = {
    ItemValue: ''
  };
  selectedExp: any = {
    ItemValue: ''
  };
  drivingLicenses: Array<any> = [];
  selectedDrivingLicense: string = '';
  workTypeSubscription: any;
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _StorageService: StorageService,
    private _ActionSheetController: ActionSheetController,
    private _EmployeeService: EmployeeService,
    private _NativeService: NativeService,
    private _AlertService: AlertService,
    private _NavParams: NavParams
  ) {
    this.userInfo = this._StorageService.read(user_info);
  }

  // 首次加载时候执行
  ionViewDidLoad() {

    if (this._NavParams.get('id')) {
      this.id = this._NavParams.get('id');
      this.getMyWorkerDetail();
    }
    this.getPositionTree();
    this.getWorkSkill();
    this.getLanguage();
    this.getOutsiteExp();
    this.getCultural();
    this.getDrivingLicenses();

    // 订阅期望职位
    this.workTypeSubscription = this._ParameterPassingService.expectWorkTypeSource$.subscribe(
      (data: any) => {
        this.types = data;
      });

    // 订阅个人简介
    this.personalIntroSubscription = this._ParameterPassingService.personalIntroSource$.subscribe(
      (data: any) => {
        this.personalIntro = data;
      });

    // 订阅个人视频
    this.personalVideoSubscription = this._ParameterPassingService.personalVideoSource$.subscribe(
      (data: any) => {
        this.videoData = data;
      });

    // 订阅个人籍贯
    this.personalHomeTownSubscription = this._ParameterPassingService.homeTownSource$.subscribe(
      (data: any) => {
        this.hometown = data;
      });
  }

  // 个人简介
  goPersonalIntroPage() {
    this.navCtrl.push(PersonalIntroPage);
  }

  // 跳转到籍贯页面
  goHomeTown() {
    this.navCtrl.push(ChooseHomeTownPage);
  }

  // 跳转到个人视频页面
  goPersonalVideoPage() {
    this.navCtrl.push(PersonalVideoPage);
  }

  // 上传头像
  uploadHeadImg() {
    const actionSheet = this._ActionSheetController.create({
      title: '请选择图片',
      buttons: [
        {
          text: '拍照',
          handler: () => {
            this._NativeService.getPictureByCamera().subscribe(img => {
              let postBody = {
                file: img
              };
              this.fileUpload(postBody);
            });
          }
        },{
          text: '手机相册',
          handler: () => {
            this._NativeService.getPictureByPhotoLibrary().subscribe(img => {
              let postBody = {
                file: img
              };
              this.fileUpload(postBody);
            });
          }
        }
      ]
    });
    actionSheet.present();
  }

  // 文件上传
  fileUpload(postBody) {
    this._EmployeeService.fileUpload(postBody).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.headImg = data.Data;
      }
    });
  }

  // 文化水平
  getWorkSkill() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_Requirement.EnumPositionType'
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
        this.exps.forEach(item => {
          if(item.ItemValue == this.selectedExp.ItemValue) {
            this.selectedExp = item;
          }
        });
      }
    });
  }

  // 文化水平
  getCultural() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_CustomerWorkmate.Cultural'
    };
    this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.culs = data.Data;
        this.culs.forEach(item => {
          if (item.ItemValue == this.cultural.ItemValue) {
            this.cultural = item;
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

  // input type=file 选中文件后执行提交操作
  onSelectChanged(evt) {
    const files: any = evt.target.files;
    if (files.length > 1) {
      this._AlertService.presentToast('一次只能上传一个附件', 2000);
      return;
    } else {
      let fileName = files[0].name;
      if (fileName.indexOf('.doc') == -1) {
        this._AlertService.presentToast('只能上传word类型附件', 2000);
        return;
      }

      let fileSize = files[0].size;
      if (fileSize / (1000 * 1024) > 10) {
        this._AlertService.presentToast('附件大小不能超过10M', 2000);
        return;
      }
    }

    let formData = new FormData();
    formData.append("file", files[0]);

    this._EmployeeService.postFile(formData).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.wordFile = data.Data;
      }
    });
  }

  // 保存工友信息
  saveWorkInfo() {
    this.submitted = true;

    // 非空效验
    if (this.selectedDrivingLicense == '' || this.customerName == '' || this.customerTel == null || this.customerSex ==  '' || this.customerAge == null || this.hometown.value == '' || this.types.length == 0){
      return;
    }

    let tempType = [];
    this.types.forEach(item => {
      tempType.push(item.id);
    });

    let postBody: any = {
      Id: this.id,
      PK_App_Customer_CustomerName: this.userInfo.Id,
      Photo: this.headImg,
      Name: this.customerName,
      Phone: this.customerTel,
      Sex: this.customerSex,
      Age: this.customerAge,
      BirthPlace: this.hometown.value,
      // CurrentPlace: "",
      Cultural: this.cultural.ItemValue ,
      EnumForeignLangGrade: this.selectedLan.ItemValue,
      SwitchBtnPassport: this.passport,
      AbroadExp: this.selectedExp.ItemValue,
      Introduction: this.personalIntro,
      WordPath: this.wordFile.path,
      WordName: this.wordFile.FileName,
      WordExt: this.wordFile.FileExt,
      VideoPath: this.videoData.path,
      JobIntension: tempType.join(','),
      EnumDriverLicence: this.selectedDrivingLicense
    };

    this._EmployeeService.createEditCustomerWorkmate(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        this._ParameterPassingService.setRefreshOption(true);
        this._AlertService.presentAlert(data.Message);
        this.navCtrl.pop();
      } else {
        this._AlertService.presentAlert(data.Message);
      }
    });
  }

  // 获取工友详情
  getMyWorkerDetail() {
    this._EmployeeService.getMyWorkerDetail(this.userInfo.Id, this.id).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        // 初始化参数
        if (data.Data.Photo != null && data.Data.Photo != '') {
          this.headImg = data.Data.Photo;
        }

        if (data.Data.Name != null && data.Data.Name != '') {
          this.customerName = data.Data.Name;
        }

        if (data.Data.Phone != null && data.Data.Phone != '') {
          this.customerTel = Number(data.Data.Phone);
        }

        if (data.Data.Sex != null && data.Data.Sex != '') {
          this.customerSex = data.Data.Sex;
        }

        if (data.Data.Age != null && data.Data.Age != '') {
          this.customerAge = Number(data.Data.Age);
        }

        if (data.Data.BirthPlace != null && data.Data.BirthPlace != '') {
          this.hometown.displayName = data.Data.BirthPlace;
          this.hometown.value = data.Data.BirthPlace;
        }

        if (data.Data.VideoPath != null && data.Data.VideoPath != '') {
          this.videoData.path = data.Data.VideoPath;
        }

        if (data.Data.WordPath != null && data.Data.WordPath != '') {
          this.wordFile = {
            FileExt: data.Data.WordExt,
            FileName: data.Data.WordName,
            path: data.Data.WordPath
          }
        }

        if (data.Data.Cultural != null && data.Data.Cultural != '') {
          this.cultural.ItemValue = data.Data.Cultural;
          this.culs.forEach(item => {
            if (item.ItemValue == this.cultural.ItemValue) {
              this.cultural = item;
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

        if (data.Data.SwitchBtnPassport != null && data.Data.SwitchBtnPassport != '') {
          this.passport = data.Data.SwitchBtnPassport;
        }

        if (data.Data.AbroadExp != null && data.Data.AbroadExp != '') {
          this.selectedExp.ItemValue = data.Data.AbroadExp;
          this.exps.forEach(item => {
            if(item.ItemValue == this.selectedExp.ItemValue) {
              this.selectedExp = item;
            }
          });
        }

        if (data.Data.Introduction != null && data.Data.Introduction != '') {
          this.personalIntro = data.Data.Introduction;
        }

        if (data.Data.EnumDriverLicence != null && data.Data.EnumDriverLicence != '') {
          this.selectedDrivingLicense = data.Data.EnumDriverLicence;
        }

        if (data.Data.JobIntension != null && data.Data.JobIntension != '') {
          let tempNameArray = data.Data.JobIntensionNames.split(',');
          let tempArray = data.Data.JobIntension.split(',');
          tempArray.forEach((item, index) => {
            this.types.push({
              name: tempNameArray[index],
              id: item
            });
          });
        }
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

  // 文化水平
  getDrivingLicenses() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_CustomerWorkmate.EnumDriverLicence'
    };
    this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.drivingLicenses = data.Data;
      }
    });
  }
}
