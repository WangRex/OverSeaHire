import { Component, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Tabs } from "ionic-angular";
import { BossHomePage } from "../home/home";
import { PublishOfferPage } from "../publish-offer/publish-offer";
import { OfferManagePage } from "../offer-manage/offer-manage";
import { BossMessageListPage } from "../message-list/message-list";
import { MyProfileBossPage } from "../my-profile/my-profile";
import { StorageService } from "../../../providers/storage-service";
import {edit_offer_detail, go_back_offer_manage_tab} from "../../../providers/constants";

@Component({
  selector: 'tab-boss-home',
  templateUrl: 'tabs.html'
})
export class BossTabsPage {
  @ViewChild('bossTabs') tabRef: Tabs;
  tab1Root = BossHomePage;
  tab2Root = OfferManagePage;
  tab3Root = PublishOfferPage;
  tab4Root = BossMessageListPage;
  tab5Root = MyProfileBossPage;

  constructor(
    private _StorageService: StorageService,
    private _ChangeDetectorRef: ChangeDetectorRef,
  ) {}

  ionViewDidLoad() {
    if (this._StorageService.readSession(edit_offer_detail)) {
      this.tabRef.select(2);
    } else if (this._StorageService.readSession(go_back_offer_manage_tab)) {
      this.tabRef.select(1);
      this._StorageService.removeSession(go_back_offer_manage_tab);
    }
    this._ChangeDetectorRef.markForCheck();
    this._ChangeDetectorRef.detectChanges();
  }
}
