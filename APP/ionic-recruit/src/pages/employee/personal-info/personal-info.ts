import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { PersonalIntroPage } from "../personal-intro/personal-intro";
import { ActionSheetController } from 'ionic-angular';
import { NativeService } from "../../../providers/native-service";
import { EmployeeService } from "../employee-service";
import {file_url, user_info} from "../../../providers/constants";
import { AlertService } from "../../../providers/alert.service";
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { PersonalVideoPage } from "../personal-video/personal-video";
import { CityData } from "../../../providers/city-data";
import { ChooseHomeTownPage } from "../choose-home-town/choose-home-town";
import { StorageService } from "../../../providers/storage-service";

@Component({
  selector: 'page-personal-info',
  templateUrl: 'personal-info.html'
})
export class PersonalInfoPage {
  userInfo: any;
  submitted: boolean = false;
  customerName: string = '';
  customerTel: number = null;
  customerSex: string = '';
  customerAge: number = null;
  customerHeight: number = null;
  submittedPlus: boolean = false;
  showFixedContent: boolean = false;
  // 添加工作经历用
  showAddWorkContent: boolean = false;
  startDate: any = null;
  endDate: any = null;
  companyName: string = '';
  position: string = '';
  works: Array<any> = []; // 工作经历
  // 添加教育经历
  showAddEducationContent: boolean = false;
  eductionWorks: Array<any> = [];
  school: string = '';
  degree: string = '';
  //添加家庭成员
  showAddUserContent: boolean = false;
  users: Array<any> = [];
  userName: string = '';
  userAge: number = null;
  userRelative: string = '';
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
  dependentColumns: any = CityData;
  hometown: any = {
    value: '',
    displayName: "请填写您的籍贯"
  };

  constructor(
    public navCtrl: NavController,
    private _ActionSheetController: ActionSheetController,
    private _NativeService: NativeService,
    private _EmployeeService: EmployeeService,
    private _AlertService: AlertService,
    private _ParameterPassingService: ParameterPassingService,
    private _StorageService: StorageService
  ) {
    this.userInfo = this._StorageService.read(user_info);
  }

  ionViewDidEnter() {
    this.showFixedContent = true;
  }

  // 页面退出的时候触发-手动隐藏掉左侧菜单，否则总会关闭的时候延迟一下
  ionViewWillLeave() {
    this.showFixedContent = false;
  }

  // 首次加载时候执行
  ionViewDidLoad() {
    this.getCustomerInfo();
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

  // 跳转到个人介绍页面
  goPersonalIntroPage() {
    this.navCtrl.push(PersonalIntroPage);
  }

  // 跳转到个人视频页面
  goPersonalVideoPage() {
    this.navCtrl.push(PersonalVideoPage);
  }

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

  fileUpload(postBody) {
    this._EmployeeService.fileUpload(postBody).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.headImg = data.Data;
      }
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

  // 点击添加工作经历
  addWork() {
    this.showAddWorkContent = true;
    this.showFixedContent = true;
  }

  // 点击添加教育经历
  addEducationWork() {
    this.showAddEducationContent = true;
    this.showFixedContent = true;
  }

  // 点击添加家庭成员
  addUser() {
    this.showAddUserContent = true;
    this.showFixedContent = true;
  }

  // 保存工作经历
  saveWork() {
    this.submittedPlus = true;
    if (this.startDate != null && this.endDate != null && this.companyName.trim() != '' && this.position != '') {
      this.works.push({
        startDate: this.startDate,
        endDate: this.endDate,
        companyName: this.companyName,
        position: this.position
      });
      this.startDate = null;
      this.endDate = null;
      this.companyName =  '';
      this.position = '';
      this.showAddWorkContent = false;
      this.showFixedContent = false;
      this.submittedPlus = false;
    }
  }

  // 保存教育经历
  saveEducationWork() {
    this.submittedPlus = true;
    if (this.startDate != null && this.endDate != null && this.school.trim() != '' && this.degree != '') {
      this.eductionWorks.push({
        startDate: this.startDate,
        endDate: this.endDate,
        school: this.school,
        degree: this.degree
      });
      this.startDate = null;
      this.endDate = null;
      this.school =  '';
      this.degree = '';
      this.showAddEducationContent = false;
      this.showFixedContent = false;
      this.submittedPlus = false;
    }
  }

  // 保存家庭成员
  saveUser() {
    this.submittedPlus = true;
    if (this.userAge != null && this.userName.trim() != '' && this.userRelative.trim() != '') {
      this.users.push({
        userName: this.userName,
        userRelative: this.userRelative,
        userAge: this.userAge
      });
      this.userAge = null;
      this.userRelative =  '';
      this.userName = '';
      this.showAddUserContent = false;
      this.showFixedContent = false;
      this.submittedPlus = false;
    }
  }

  // date: yyyy-MM
  formatDateDisplay(date) {
    let tempArray = date.split('-');
    return tempArray[0] + '年' + tempArray[1] + '月';
  }

  saveCustomerInfo() {
    this.submitted = true;

    // 非空效验
    if (this.customerName == '' || this.customerTel == null || this.customerSex ==  '' || this.customerAge == null || this.customerHeight == null || this.hometown.value == ''){
      return;
    }

    let workExpPosts = [];
    this.works.forEach(item => {
      workExpPosts.push({
        PK_App_Customer_CustomerName: this.userInfo.Id,
        StartDate: item.startDate,
        EndDate: item.endDate,
        Company: item.companyName,
        Position: item.position
      });
    });

    let postBody: any = {
      Id: this.userInfo.Id,
      Photo: this.headImg,
      CustomerName: this.customerName,
      Phone: this.customerTel,
      Sex: this.customerSex,
      Age: this.customerAge,
      Height: this.customerHeight,
      // Weight: "string",
      // Nation: "string",
      // Introduction: "string",
      WordPath: this.wordFile.path,
      WordName: this.wordFile.FileName,
      WordExt: this.wordFile.FileExt,
      VideoPath: this.videoData.path,
      BirthPlace: this.hometown.value,
      workExpPosts: workExpPosts,
      // eduExpPosts: [
      //   {
      //     "PK_App_Customer_CustomerName": "string",
      //     "StartDate": "string",
      //     "EndDate": "string",
      //     "School": "string",
      //     "Degree": "string"
      //   }
      // ],
      // familyPosts: [
      //   {
      //     "PK_App_Customer_CustomerName": "string",
      //     "Name": "string",
      //     "Age": 0,
      //     "Relation": "string"
      //   }
      // ]
    };
    this._EmployeeService.updateCustomerInfo(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        this.userInfo.CustomerName = this.customerName;
        this.userInfo.Phone  = this.customerTel;
        this.userInfo.Photo  = this.headImg;
        this.userInfo.IntroFlag = true;
        this._StorageService.write(user_info, this.userInfo);
        this.submitted = false;
        this._AlertService.presentAlert(data.Message);
        this.navCtrl.pop();
      } else {
        this._AlertService.presentAlert(data.Message);
      }
    });
  }

  // 跳转到籍贯页面
  goHomeTown() {
    this.navCtrl.push(ChooseHomeTownPage);
  }

  // 获取个人信息
  getCustomerInfo() {
    this._EmployeeService.getCustomerInfo(this.userInfo.Id).subscribe((data: any) => {
      if (data.Code == 200) {
        // 初始化参数
        if (data.Data.Photo != null && data.Data.Photo != '') {
          this.headImg = data.Data.Photo;
        }

        if (data.Data.CustomerName != null && data.Data.CustomerName != '') {
          this.customerName = data.Data.CustomerName;
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

        if (data.Data.Height != null && data.Data.Height != '') {
          this.customerHeight = Number(data.Data.Height);
        }

        if (data.Data.BirthPlace != null && data.Data.BirthPlace != '') {
          this.hometown.displayName = data.Data.BirthPlace;
          this.hometown.value = data.Data.BirthPlace;
        }

        if (data.Data.VideoPath != null && data.Data.VideoPath != '') {
          this.videoData.path = data.Data.VideoPath;
        }

        data.Data.workExpPosts.forEach(item => {
          this.works.push({
            startDate: item.StartDate,
            endDate: item.EndDate,
            companyName: item.Company,
            position: item.Position
          });
        });

        if (data.Data.WordPath != null && data.Data.WordPath != '') {
          this.wordFile = {
            FileExt: data.Data.WordExt,
            FileName: data.Data.WordName,
            path: data.Data.WordPath
          }
        }

      } else {
        this._AlertService.presentToast(data.Message);
      }
    });
  }
}
