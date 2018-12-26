import {Component} from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import { EmployeeService } from "../employee-service";
import { AlertService } from "../../../providers/alert.service";

@Component({
  selector: 'page-choose-city',
  templateUrl: 'choose-city.html'
})
export class ChooseCityPage {
  types: Array<any> = [];
  selectedTypes: Array<any> = [];
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _EmployeeService: EmployeeService,
    private _AlertService: AlertService,
    private _NavParams: NavParams
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.getWorkCities();
  }

  save() {
    if (this.selectedTypes.length == 0) {
      this._AlertService.presentAlert('请选择期望国家');
    } else {
      this._ParameterPassingService.setExpectCitySource(this.selectedTypes);
      this.navCtrl.pop();
    }
  }

  setSelectedType(option) {
      if (option.selected == true) {
        this.selectedTypes.splice(this.selectedTypes.indexOf(option), 1);
        option.selected = false;
      } else {
        if (this.selectedTypes.length >= 3) {
          this._AlertService.presentAlert('最多只能选三个');
        } else {
          option.selected = true;
          this.selectedTypes.push(option);
        }
      }
  }

  getWorkCities() {

    let types = this._NavParams.get('types');
    this._EmployeeService.getCountries().subscribe((data: any) => {
      if (data.Code == 200 || data.Code == 0) {
        this.types = data.Data;
        types.forEach(type => {
          this.types.forEach(item => {
            if (type.Id == item.Id) {
              item.selected = true;
              this.selectedTypes.push(item);
            }
          });
        });

      }
    });
  }
}
