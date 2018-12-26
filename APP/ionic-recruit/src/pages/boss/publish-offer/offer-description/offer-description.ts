import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ParameterPassingService } from "../../../../providers/rxjs-parameter-passing";
import { AlertService } from "../../../../providers/alert.service";

@Component({
  selector: 'page-offer-description',
  templateUrl: 'offer-description.html'
})
export class OfferDescriptionPage {
  description: string = '';
  tags: Array<any> = [];
  tagName: string = '';
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService,
    private _NavParams: NavParams
  ) {
    if (this._NavParams.get('offerDetail')) {
      let detail: any = this._NavParams.get('offerDetail');
      if (detail.length > 0) {
        this.tags  = detail[0].tags;
        this.description = detail[0].description;
      }
    }
  }

  saveOfferDescription() {
    if (this.description.trim() == '') {
      this._AlertService.presentToast('职位描述不能为空');
      return;
    } else {
      this._ParameterPassingService.setOfferDescriptionSource({
        description: this.description,
        tags: this.tags
      });
      this.navCtrl.pop();
    }
  }

  addTag() {
    if (this.tagName.trim() == '') {
      this._AlertService.presentToast('请输入标签名');
    } else {
      this.tags.push(this.tagName);
      this.tagName = '';
    }
  }

  removeTag(index) {
    this.tags.splice(index, 1)
  }

}
