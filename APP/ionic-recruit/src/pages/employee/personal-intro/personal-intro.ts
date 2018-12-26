import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-personal-intro',
  templateUrl: 'personal-intro.html'
})
export class PersonalIntroPage {
  description: string = '';
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _AlertService: AlertService
  ) {}

  savePersonalIntro() {
    if (this.description.trim() == '') {
      this._AlertService.presentToast('个人简介不能为空');
      return;
    } else {
      this._ParameterPassingService.setPersonalIntroOption(this.description);
      this.navCtrl.pop();
    }
  }
}
