import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { MyApp } from './app.component';
import { EmployeeModule } from "../pages/employee/employee.module";
import { BossModule } from "../pages/boss/boss.module";

import { ChooseRolePage } from "../pages/choose-role/choose-role";
import { Login } from "../pages/login/login";
import { SettingPage } from "../pages/setting/setting";
import { FeedbackPage } from "../pages/feedback/feedback";
import { ChooseWorkTypePage } from "../pages/choose-work-type/choose-work-type";

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { HttpClientModule } from '@angular/common/http';
// providers
import { AlertService } from "../providers/alert.service";
import { HttpService } from "../providers/http-service";
import { StorageService } from "../providers/storage-service";
import { Utils } from "../providers/utils";
import { CustomValidators } from "../providers/validators";
import { LoginService } from "../pages/login/login-service";
import { Camera } from "@ionic-native/camera";
import { FeedbackService } from "../pages/feedback/feedback-service";
import { InAppBrowser } from '@ionic-native/in-app-browser';
import { Geolocation } from '@ionic-native/geolocation';

@NgModule({
  declarations: [
    MyApp,
    Login,
    ChooseRolePage,
    SettingPage,
    FeedbackPage,
    ChooseWorkTypePage
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    IonicModule.forRoot(MyApp, {
      backButtonText: '',
      iconMode: 'ios',
      backButtonIcon: 'ios-arrow-back',
      mode: 'ios',
      tabsHideOnSubPages: true
    }),
    EmployeeModule,
    BossModule
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    Login,
    ChooseRolePage,
    SettingPage,
    FeedbackPage,
    ChooseWorkTypePage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    Camera,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    AlertService,
    Utils,
    HttpService,
    StorageService,
    CustomValidators,
    LoginService,
    FeedbackService,
    InAppBrowser,
    Geolocation
  ]
})
export class AppModule {}
