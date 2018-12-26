import {Component, ViewChild } from '@angular/core';
import { NavController} from 'ionic-angular';
import { Content } from 'ionic-angular';
import { UserInfoPage } from "../user-info/user-info";
import { SearchUserPage } from "../search-users/search-user";
import { BossService } from "../boss-service";
import { user_info_boss, page_size } from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";
import { Login } from "../../login/login";

@Component({
  selector: 'page-boss-home',
  templateUrl: 'home.html'
})
export class BossHomePage {
  @ViewChild(Content) content: Content;
  loadMore: boolean = false;
  pageNum: number = 1;
  recommend: string = '';
  latest: string = '';
  data: Array<any> = [];
  userInfo: any;

  constructor(
    public navCtrl: NavController,
    private _BossService: BossService,
    private _StorageService: StorageService,
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {}

  ionViewDidEnter() {
    if (!this._StorageService.read(user_info_boss)) {
      this.navCtrl.push(Login);
    } else {
      this.userInfo = this._StorageService.read(user_info_boss);
      this.getUserList();
    }
  }

  getUserList(event?) {
    let postBody = {
      UserId: this.userInfo.Id,
      IsRecommend: this.recommend,
      IsLatest: this.latest,
      EmployerId: this.userInfo.Id,
      PageNum: this.pageNum,
      RecordNum: page_size

    };
    this._BossService.getRecommendUserList(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        data.Data.applyJobUserVms.forEach(item => {
          item.description = item.Sex + '/' + item.Age + '/' + item.DriverLicence + '/' + item.JobIntensionName + '/' + item.BirthPlace;
          if (item.AbroadExp != '' && item.AbroadExp != null) {
            item.description += '/' + item.AbroadExp;
          }
        });
        if (this.pageNum == 1) {
          this.data = data.Data.applyJobUserVms;
        } else {
          this.data = this.data.concat(data.Data.applyJobUserVms);
        }
        this.loadMore = this.data.length < data.DataCount ? true : false;
      }

      if (event) {
        event.complete();
      }
    });
  }

  // 下拉刷新
  doInfinite(infiniteScroll) {
    this.pageNum++;
    this.getUserList(infiniteScroll);
  }

  switchUserList(flag) {
    this.pageNum = 1;
    this.loadMore = false;
    // 第一页让内容滚动到最上面
    this.data = [];
    this.content.scrollToTop(0);
    this.content.resize();
    if (flag == 'recommend') {
      this.recommend = '';
      this.latest = '';
    }

    if (flag == 'latest') {
      this.recommend = '';
      this.latest = '1';
    }
    this.getUserList();
  }

  goUserDetail(id) {
    this.navCtrl.push(UserInfoPage, {
      workerId: id
    });
  }

  goSearchUserPage() {
    this.navCtrl.push(SearchUserPage);
  }
}
