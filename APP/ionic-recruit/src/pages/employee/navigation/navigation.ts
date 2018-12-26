import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertService } from "../../../providers/alert.service";
import { Geolocation } from '@ionic-native/geolocation';
import { HomePage } from "../home/home";

declare var AMap;

@Component({
  selector: 'page-navigation',
  templateUrl: 'navigation.html'
})
export class NavigationPage {
  markers: Array<any> = [];
  shop: any;
  constructor(
    private _NavParams: NavParams,
    private _AlertService: AlertService,
    private geolocation: Geolocation,
    private _NavController: NavController
  ) {
    this.shop = this._NavParams.get('shop');
    if (this.shop) {
      this.markers = [this.shop];
    }
  }

  ionViewDidLoad() {

  }

  navigateToShop() {
    this.geolocation.getCurrentPosition().then((data) => {
      this.openAmap(data.coords.longitude, data.coords.latitude, this.shop.Longitude, this.shop.Latitude);
    }).catch((error) => {
        this._AlertService.presentToast('获取当前定位失败，请检查手机定位权限是否开启');
    });
  }

  openAmap(startLng, startLat, endLng, endLat) {
    let map = new AMap.Map("mapContainerCoopHome");
    AMap.plugin(["AMap.Driving"], function () {
      var drivingOption = {
        policy: AMap.DrivingPolicy.LEAST_TIME,
        map: map
      };
      var driving = new AMap.Driving(drivingOption); //构造驾车导航类
      //根据起终点坐标规划驾车路线
      driving.search([startLng, startLat], [endLng, endLat], function (status, result) {
        driving.searchOnAMAP({
          origin: result.origin,
          destination: result.destination
        });
      });
    });
  }

  navigateToHome() {
    this._NavController.popToRoot();
  }
}
