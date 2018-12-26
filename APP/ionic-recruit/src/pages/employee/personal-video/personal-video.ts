import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { AlertService } from "../../../providers/alert.service";
import { ElementRef } from "@angular/core";
import { EmployeeService } from "../employee-service";
import { file_url } from "../../../providers/constants";
import { InAppBrowser } from '@ionic-native/in-app-browser';
import { Clipboard } from '@ionic-native/clipboard';

@Component({
  selector: 'page-personal-video',
  templateUrl: 'personal-video.html'
})
export class PersonalVideoPage {
  videoData: any = { path: '' };
  videoInstance: any;
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService,
    private _ElementRef: ElementRef,
    private _EmployeeService: EmployeeService,
    private _InAppBrowser: InAppBrowser,
    private clipboard: Clipboard
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {

    // 暂时换成URL
    // this.videoInstance = this._ElementRef.nativeElement.querySelector('#personalVideo');
  }

  savePersonalIntro() {
    if (this.videoData.path.trim() == '') {
      this._AlertService.presentToast('请输入视频地址');
      // this._AlertService.presentToast('请选择工作视频');
      return;
    } else {
      this._ParameterPassingService.setPersonalVideoSource(this.videoData);
      this.navCtrl.pop();
    }
  }


  // input type=file 选中文件后执行提交操作
  onSelectChanged(evt) {
    const files: any = evt.target.files;
    if (files.length > 1) {
      this._AlertService.presentToast('一次只能上传一个附件', 2000);
      return;
    } else {
      let fileName = files[0].name;
      if (fileName.indexOf('.mp4') == -1) {
        this._AlertService.presentToast('只能上传.mp4类型的视频文件', 2000);
        return;
      }

      let fileSize = files[0].size;
      if (fileSize / (1000 * 1024) > 100) {
        this._AlertService.presentToast('视频大小不能超过100M', 2000);
        return;
      }
    }



    let formData = new FormData();
    formData.append("file", files[0]);

    this._EmployeeService.postFile(formData).subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.videoData = data.Data;
        this.videoInstance.src = file_url + this.videoData.path;
        this.videoInstance.load();
      }
    });
  }

  // 预览视频，再浏览器里打开
  previewVideo() {
    if (this.videoData.path.trim() == '') {
      this._AlertService.presentToast('请输入视频地址');
      return;
    }  else {
      this._InAppBrowser.create(this.videoData.path, '_blank');
    }

  }

  paste() {
    this.clipboard.paste().then(
      (resolve: string) => {
        if (resolve != '') {
          this.videoData.path += resolve + "\r\n";
        } else {
          this._AlertService.presentToast('剪贴板内容为空，请先复制视频地址');
        }
    });
  }

  clearPaste() {
    this.clipboard.clear();
  }
}
