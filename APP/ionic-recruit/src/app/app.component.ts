import { Component } from '@angular/core';
import { Platform } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';

import { ChooseRolePage } from "../pages/choose-role/choose-role";

@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  rootPage:any = ChooseRolePage;

  constructor(platform: Platform, statusBar: StatusBar, splashScreen: SplashScreen) {
    platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      statusBar.styleDefault();
      // statusBar.styleLightContent();
      splashScreen.hide();
      statusBar.backgroundColorByHexString('#ffffff');
      // statusBar.overlaysWebView(true);
    });
  }
}
