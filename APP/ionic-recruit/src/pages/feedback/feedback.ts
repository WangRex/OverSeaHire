import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { StorageService } from "../../providers/storage-service";
import { AlertService } from "../../providers/alert.service";
import { ActionSheetController } from 'ionic-angular';
import { NativeService } from "../../providers/native-service";
import { FeedbackService } from "./feedback-service";
import {user_info} from "../../providers/constants";

@Component({
  selector: 'page-feedback',
  templateUrl: 'feedback.html'
})
export class FeedbackPage {
  description: string = '';
  img: string = '';
  constructor(
    public navCtrl: NavController,
    private _ActionSheetController: ActionSheetController,
    private _NativeService: NativeService,
    private _StorageService: StorageService,
    private _AlertService: AlertService,
    private _FeedbackService: FeedbackService
  ) {

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
    this._FeedbackService.fileUpload(postBody).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.img = data.Data;
      }
    });
  }

  removeImg() {
    this.img = '';
  }

  saveFeedback() {
    if (this.description.trim() == '') {
      this._AlertService.presentToast('请输入您的宝贵意见');
      return;
    }

    let userInfo: any = this._StorageService.read(user_info);
    let postBody = {
      PK_App_Customer_CustomerName: userInfo.Id,
      ImgList: this.img,
      Content: this.description
    };

    this._FeedbackService.createSysFeedback(postBody).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this._AlertService.presentToast(data.Message);
        this.navCtrl.pop();
      } else {
        this._AlertService.presentToast(data.Message);
      }
    });
  }
}
