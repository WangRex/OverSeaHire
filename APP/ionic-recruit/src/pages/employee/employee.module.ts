import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ComponentsModule } from "../../components/components.module";
import { EmployeeComponent } from "./employee.component";
import { HomePage } from "./home/home";
import { EmployeeTabsPage } from "./tabs/tabs";
import { OfferDetailPage } from "./offer-detail/offer-detail";
import { ApplyOfferPage } from "./apply-offer/apply-offer";
import { MyWorkerPage } from "./my-worker/my-worker";
import { ApplyCompletePage } from "./apply-complete/apply-complete";
import { PaymentPage } from "./payment/payment";
import { AttentionsPage } from "./attentions/attentions";
import { OrderListPage } from "./order-list/order-list";
import { OrderDetailPage } from "./order-detail/order-detail";
import { SearchOfferPage } from "./search-offer/search-offer";
import { MessageListPage } from "./message-list/message-list";
import { MyProfilePage } from "./my-profile/my-profile";
import { PersonalInfoPage } from "./personal-info/personal-info";
import { PersonalIntroPage } from "./personal-intro/personal-intro";
import { ExpectOfferPage } from "./expect-offer/expect-offer";
import { AddWorkerPage } from "./add-worker/add-worker";
import { MyFavoriteOfferPage } from "./my-favorite-offer/my-favorite-offer";
import { EmployeeService } from "./employee-service";
import { Camera } from "@ionic-native/camera";
import { NativeService } from "../../providers/native-service";
// import { NgxUploaderModule } from 'ngx-uploader';
import { ParameterPassingService } from "../../providers/rxjs-parameter-passing";
import { PersonalVideoPage } from "./personal-video/personal-video";
import { ChooseHomeTownPage } from "./choose-home-town/choose-home-town";
import { ChooseCityPage } from "./choose-city/choose-city";
import { Clipboard } from '@ionic-native/clipboard';
import { MyAppliedOfferPage } from "./my-applied-offer/my-applied-offer";
import { MyInterviewOfferPage } from "./my-interview-offer/my-interview-offer";
import { MyRecommendOfferPage } from "./my-recommend-offer/my-recommend-offer";
import { CurrentOrderStepsPage } from "./current-order-steps/current-order-steps";
import { InAppBrowser } from '@ionic-native/in-app-browser';
import { NavigationPage } from "./navigation/navigation";
import { Geolocation } from '@ionic-native/geolocation';

const components = [
  HomePage,
  EmployeeTabsPage,
  OfferDetailPage,
  ApplyOfferPage,
  MyWorkerPage,
  ApplyCompletePage,
  PaymentPage,
  AttentionsPage,
  OrderListPage,
  OrderDetailPage,
  SearchOfferPage,
  MessageListPage,
  MyProfilePage,
  PersonalInfoPage,
  PersonalIntroPage,
  ExpectOfferPage,
  AddWorkerPage,
  MyFavoriteOfferPage,
  PersonalVideoPage,
  ChooseHomeTownPage,
  ChooseCityPage,
  MyAppliedOfferPage,
  MyInterviewOfferPage,
  MyRecommendOfferPage,
  CurrentOrderStepsPage,
  NavigationPage
];

@NgModule({
  declarations: [
    EmployeeComponent,
    components
  ],
  imports: [
    IonicPageModule.forChild(EmployeeComponent),
    ComponentsModule,
  ],
  entryComponents: [
    components
  ],
  exports: [
    EmployeeComponent
  ],
  providers: [
    Camera,
    EmployeeService,
    NativeService,
    ParameterPassingService,
    Clipboard,
    InAppBrowser,
    Geolocation
  ]
})

export class EmployeeModule {
}
