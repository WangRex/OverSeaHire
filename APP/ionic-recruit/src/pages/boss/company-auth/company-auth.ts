import { Component } from '@angular/core';
import { NavController, ActionSheetController } from 'ionic-angular';
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { AlertService } from "../../../providers/alert.service";
import {file_url, user_info_boss} from "../../../providers/constants";
import { NativeService } from "../../../providers/native-service";
import { BossService } from "../boss-service";
import { StorageService } from "../../../providers/storage-service";

@Component({
  selector: 'page-company-auth',
  templateUrl: 'company-auth.html'
})
export class CompanyAuthPage {
  fileUrl = file_url;
  companyName: string = '';
  companyDes: string = '';
  fields: Array<any> = [];
  selectedField: string = '';
  companySizes: Array<any> = [];
  selectedCompanySize: string = '';
  companyImg: string = '';
  userInfo: any;
  submitted: boolean = false;
  id: string = '';
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService,
    private _ActionSheetController: ActionSheetController,
    private _NativeService: NativeService,
    private _BossService: BossService,
    private _StorageService: StorageService
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.userInfo = this._StorageService.read(user_info_boss);
    this.getCompanySize();
    this.getIndustry();
    this.getCompanyInfo();
  }

  // 提交认证
  updateCompany() {
    this.submitted = true;
    if (this.companyName == '' || this.companyDes == '' || this.selectedField == '' || this.selectedCompanySize == '') {
      return;
    }

    if (this.companyImg == '') {
      this._AlertService.presentToast('请上传公司营业执照');
      return;
    }

    let postBody = {
      Id: this.id,
      UserId: this.userInfo.Id,
      CompanyName: this.companyName,
      CompanyShortName: this.companyDes,
      Industry: this.selectedField,
      EnumCompanySize: this.selectedCompanySize,
      BusinessLicence: this.companyImg,
      SwitchBtnApply: '0', // 0是关闭,
      PK_App_Customer_CustomerName: this.userInfo.Id
    };
    this._BossService.updateCompany(postBody).subscribe((data: any) => {
      this._AlertService.presentToast(data.Message);
      if (data.Code == 200 || data.Code == 0) {
        this.navCtrl.pop();
      }
    });
  }

  // 上传营业执照
  uploadCompanyImg() {
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
    this._BossService.fileUpload(postBody).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.companyImg = data.Data;
      }
    });
  }

  // 获取公司规模
  getCompanySize() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_Company.EnumCompanySize'
    };
    this._BossService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.companySizes = data.Data;
      }
    });
  }

  // 获取行业枚举
  getIndustry() {
    let postBody = {
      CustomerID: this.userInfo.Id,
      TableName: 'App_Company.Industry'
    };
    this._BossService.getDics(postBody, false).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.fields = data.Data;
      }
    });
  }

  // 获取行业枚举
  getCompanyInfo() {
    this._BossService.getCompany(this.userInfo.Id).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.id = data.Data.Id;
        this.companyName = data.Data.CompanyName;
        this.companyDes = data.Data.CompanyShortName;
        this.selectedField = data.Data.Industry;
        this.selectedCompanySize = data.Data.EnumCompanySize;
        this.companyImg = data.Data.BusinessLicence;
      }
    });
  }
}
