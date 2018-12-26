import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ComponentsModule } from "../../components/components.module";
import { BossComponent } from "./boss.component";
import { BossHomePage } from "./home/home";
import { BossService } from "./boss-service";
import { BossTabsPage } from "./tabs/tabs";
import { PublishOfferPage } from "./publish-offer/publish-offer";
import { UserInfoPage } from "./user-info/user-info";
import { SearchUserPage } from "./search-users/search-user";
import { OfferManagePage } from "./offer-manage/offer-manage";
import { BossOfferDetailPage } from "./offer-detail/offer-detail";
import { BossMessageListPage } from "./message-list/message-list";
import { InterviewWorkerPage } from "./interview-worker/interview-worker";
import { InterviewDetailPage } from "./interview-detail/interview-detail";
import { MyProfileBossPage } from "./my-profile/my-profile";
import { OfferDescriptionPage } from "./publish-offer/offer-description/offer-description";
import { CompanyAuthPage } from "./company-auth/company-auth";
import { InAppBrowser } from '@ionic-native/in-app-browser';
import { Clipboard } from '@ionic-native/clipboard';
import { Camera } from "@ionic-native/camera";
import { NativeService } from "../../providers/native-service";

const components = [
  BossHomePage,
  BossTabsPage,
  PublishOfferPage,
  UserInfoPage,
  SearchUserPage,
  OfferManagePage,
  BossOfferDetailPage,
  BossMessageListPage,
  InterviewWorkerPage,
  InterviewDetailPage,
  MyProfileBossPage,
  OfferDescriptionPage,
  CompanyAuthPage
];

@NgModule({
  declarations: [
    BossComponent,
    components
  ],
  imports: [
    IonicPageModule.forChild(BossComponent),
    ComponentsModule,
  ],
  entryComponents: [
    components
  ],
  exports: [
    BossComponent
  ],
  providers: [
    BossService,
    InAppBrowser,
    Clipboard,
    Camera,
    NativeService
  ]
})

export class BossModule {
}
