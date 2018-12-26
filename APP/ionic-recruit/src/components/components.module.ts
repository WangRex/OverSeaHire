import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
// component
import { SingleSelectComponent } from "./SingleSelectComponent/SingleSelectComponent";
import { Divider } from "./divider/divider";
import { UserInfo } from "./user-info/user-info";
import { OfferDetail } from "./offer-detail/offer-detail";
import { InfoPrimary } from "./info-primary/info-primary";
import { Steps } from "./steps/steps";
import { NoRecordFound } from "./no-record-found/no-record-found";
import { ProfileOption } from "./profile-option/profile-option";
import { OfferItem } from "./offer-item/offer-item";
import { BossOfferItem } from "./boss-offer-item/boss-offer-item";
import { SystemMessage } from "./system-message/system-message";
import { FlowMessage } from "./flow-message/flow-message";
import { AMapComponent } from "./amap/amap.component";

export const components = [
  SingleSelectComponent,
  Divider,
  UserInfo,
  OfferDetail,
  InfoPrimary,
  Steps,
  NoRecordFound,
  ProfileOption,
  OfferItem,
  BossOfferItem,
  SystemMessage,
  FlowMessage,
  AMapComponent
];

@NgModule({
  declarations: [components],
  imports: [IonicModule],
  exports: [components],
  providers: []
})
export class ComponentsModule { }
