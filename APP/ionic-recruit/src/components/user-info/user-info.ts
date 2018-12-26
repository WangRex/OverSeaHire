import { Component,Input } from '@angular/core';
import { file_url } from "../../providers/constants";
import { Clipboard } from '@ionic-native/clipboard';
import { AlertService } from "../../providers/alert.service";

@Component({
    selector: 'ion-user-info',
    templateUrl: 'user-info.html',
})

export class UserInfo {
    fileUrl = file_url;
    defaultImg: string = 'assets/imgs/default-head.png';
    @Input() name: string = '';
    @Input() description: string = '';
    @Input() img: string = '';
    @Input() showContact: boolean = false;
    @Input() contactPhone: string = '';
    @Input() weChat: string = '';
    @Input() userPhone: string = '';
    constructor(
      private _Clipboard: Clipboard,
      private _AlertService: AlertService
    ) {
    }

  copyWechat() {
    this._Clipboard.copy(this.weChat);
    this._AlertService.presentToast('复制成功');
  }
}
