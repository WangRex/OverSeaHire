import {Component, ViewChild } from '@angular/core';
import {NavController} from 'ionic-angular';
import {OfferDetailPage} from "../offer-detail/offer-detail";
import {SearchOfferPage} from "../search-offer/search-offer";
import { EmployeeService } from "../employee-service";
import { Content } from 'ionic-angular';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  @ViewChild(Content) content: Content;
  loadMore: boolean = false;
  pageNum: number = 1;
  recommend: string = '1';
  latest: string = '';
  highSalary: string = '';
  data: Array<any> = [];

  constructor(
    public navCtrl: NavController,
    private _EmployeeService: EmployeeService,
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    // 默认获取推荐列表
    this.getRequirementList();
  }

  goOfferDetailPage(id) {
    this.navCtrl.push(OfferDetailPage, {
      id: id
    });
  }

  goSearchOfferPage() {
    this.navCtrl.push(SearchOfferPage);
  }

  getRequirementList(event?) {
    this._EmployeeService.getRequirementList(this.recommend, this.latest, this.highSalary, '', this.pageNum).subscribe((data: any) => {
      if (data.Code == 200) {
        if (this.pageNum == 1) {
          this.data = data.Data.requirementVms;
        } else {
          this.data = this.data.concat(data.Data.requirementVms);
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
    this.getRequirementList(infiniteScroll);
  }

  switchRequirementList(flag) {
    this.pageNum = 1;
    this.loadMore = false;
    // 第一页让内容滚动到最上面
    this.data = [];
    this.content.scrollToTop(0);
    this.content.resize();
    if (flag == 'recommend') {
      this.recommend = '1';
      this.latest = '';
      this.highSalary = '';
    }

    if (flag == 'latest') {
      this.recommend = '';
      this.latest = '1';
      this.highSalary = '';
    }

    if (flag == 'highSalary') {
      this.recommend = '';
      this.latest = '';
      this.highSalary = '1';
    }
    this.getRequirementList();
  }
}
