import {Component, ViewChild} from '@angular/core';
import { NavController } from 'ionic-angular';
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { CityData } from "../../../providers/city-data";
import { Content } from 'ionic-angular';

@Component({
  selector: 'page-choose-home-town',
  templateUrl: 'choose-home-town.html'
})
export class ChooseHomeTownPage {
  @ViewChild(Content) content: Content;
  provinces: Array<any> = CityData[0].options;
  cities: Array<any> = [];
  selectedProvince: any;
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
  ) {}

  setSelectedProvince(option) {
    this.selectedProvince = option;
    CityData[1].options.forEach(item => {
      if (option.value == item.parentVal) {
        this.cities.push(item);
      }
    });
    this.content.scrollToTop(0);
    this.content.resize();
  }

  setSelectedCity(option) {
    let postBody = {
      // province: this.selectedProvince.value,
      // city: option.value,
      value: this.selectedProvince.text +  ' ' + option.text,
      displayName: this.selectedProvince.text +  ' ' + option.text
    };
    this._ParameterPassingService.setHomeTownSource(postBody);
    this.navCtrl.pop();
  }
}
