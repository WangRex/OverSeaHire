import { Component, ViewChild, ElementRef  } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Content } from "ionic-angular";
import { BossService } from "../boss-service";
import { AlertService } from "../../../providers/alert.service";
import { StorageService } from "../../../providers/storage-service";
import {search_history_boss, search_work_types, user_info_boss} from "../../../providers/constants";
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";
import {ChooseWorkTypePage} from "../../choose-work-type/choose-work-type";
import {UserInfoPage} from "../user-info/user-info";

@Component({
  selector: 'page-search-user',
  templateUrl: 'search-user.html'
})
export class SearchUserPage {
  @ViewChild(Content) content: Content;
  headerOffsetH: any;
  keyword: string = '';
  showFixedContent: boolean = true;
  data: Array<any> = [];
  loadMore: boolean = false;
  pageNum: number = 1;
  hotSearch: Array<any> = [];
  searched: boolean = false;
  searchHistory: Array<any> = [];
  sorts: Array<any> = [];
  showSort: boolean = false;
  isHaveVideo: string = '';
  intelligenceSort: string = '';
  selectedWorkTypes: Array<any> = [];
  workTypeSubscription: any;
  workTypes: Array<any> = [];
  isRecommend: string = '';
  isLatest: string = '';
  showRecommend: boolean = false;

  constructor(
    public navCtrl: NavController,
    private _ElementRef: ElementRef,
    private _BossService: BossService,
    private _AlertService: AlertService,
    private _StorageService: StorageService,
    private _ParameterPassingService: ParameterPassingService
  ) {
    this.sorts = [{
      value: 'Age',
      text:  '年龄优先'
    },{
      value: 'DriverLicence',
      text:  '驾照优先'
    },{
      value: 'AbroadExp',
      text:  '出国经验优先'
    }]
  }

  ionViewDidLoad() {
    // this.getHotSearch();
    if (this._StorageService.read(search_history_boss)) {
      this.searchHistory = this._StorageService.read(search_history_boss);
    }

    // 订阅期望职位
    this.workTypeSubscription = this._ParameterPassingService.expectWorkTypeSource$.subscribe(
      (data: any) => {
        this.selectedWorkTypes = data;
        this.keyword = data[0].name;
        this.getUserList();
      });
  }

  ionViewDidEnter() {
    this.showFixedContent = true;
  }

  // 页面退出的时候触发-手动隐藏掉左侧菜单，否则总会关闭的时候延迟一下
  ionViewWillLeave() {
    this.showFixedContent = false;
  }

  getUserList(event?) {

    if (this.keyword.length < 2) {
      this._AlertService.presentToast('职位/公司关键字长度不能少于2个字', 2000, 'bottom');
      return;
    }

    // 如果搜索历史里没有过关键字直接添加到数组最前面
    if (this.keyword != '') {
      if (this.searchHistory.indexOf(this.keyword) == -1) {
        this.searchHistory.unshift(this.keyword);
      } else {
        // 如果已有关键字，先删除原来位置关键字然后把最新关键字添加到最前面
        this.searchHistory.splice(this.searchHistory.indexOf(this.keyword), 1);
        this.searchHistory.unshift(this.keyword);
      }
      if (this.searchHistory.length > 10) {
        this.searchHistory.pop();
      }
      this._StorageService.write(search_history_boss, this.searchHistory);
    }

    let tempArr = [];
    this.selectedWorkTypes.forEach(item => {
      tempArr.push(item.id);
    });

    let userInfo: any = this._StorageService.read(user_info_boss);
    let postBody = {
      UserId: userInfo.Id,
      EmployerId: userInfo.Id,
      QueryStr: this.keyword,
      PK_App_Position_Name: tempArr.join(','),
      IsRecommend: this.isRecommend,
      IsLatest: this.isLatest,
      HaveVideo: this.isHaveVideo,
      IntelligenceSort: this.intelligenceSort,
      PageNum: 0,
      RecordNum: 0
    };

    this._BossService.getRecommendUserList(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        this.searched = true;
        this.workTypes = data.Data.positionTreeVms;
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
      this.content.resize();
      // if (this.data.length > 0) {
        setTimeout(() => {
          this.headerOffsetH = this._ElementRef.nativeElement.querySelector(".search-header").offsetHeight;
        }, 100);
      // }

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

  // 获取热搜列表
  // getHotSearch() {
  //   let postBody = {
  //     PageNum: 1,
  //     RecordNum: 15,
  //     UserId: ''
  //   };
  //   // this._EmployeeService.getHotSearch(postBody).subscribe((data: any) => {
  //   //   if (data.Code == 200) {
  //   //     this.hotSearch = data.Data;
  //   //   }
  //   // });
  // }

  // 选中热门/历史搜索
  // selectHotSearch(condition) {
  //   this.keyword = condition;
  //   this.pageNum = 1;
  //   this.searchRequirementList();
  // }

  // 清除历史记录
  clearSearchHistory() {
    this.searchHistory = [];
    this._StorageService.write(search_history_boss, []);
  }

  // 打开排序面板
  toggleSort() {
    this.showSort = !this.showSort;
  }

  // 设置选中的排序条件
  setSelectedSortOption(option) {
    this.intelligenceSort = option.value;
  }

  // 确认按照选中的排序进行搜索
  confirmSelectedSort() {
    this.showSort = false;
    this.getUserList();
  }

  // 打开或者关闭推荐显示内容
  toggleRecommendSearch() {
    this.showRecommend = !this.showRecommend;
  }

  // 确认按照选中的推荐进行搜索
  confirmRecommendSearch() {
    this.showRecommend = false;
    this.getUserList();
  }

  // 选中推荐选项，系统推荐，最新，有视频简历
  selectRecommendOption(option) {
    if (option == 'isRecommend') {
      this.isRecommend = '1';
      this.isLatest = '';
      this.isHaveVideo = '';
    }

    if (option == 'isLatest') {
      this.isRecommend = '';
      this.isLatest = '1';
      this.isHaveVideo = '';
    }

    if (option == 'isHaveVideo') {
      this.isRecommend = '';
      this.isLatest = '';
      this.isHaveVideo = '1';
    }
  }

  // 跳转到选择工种页面
  goChooseWorkTypePage() {
    this._StorageService.writeSession(search_work_types, this.workTypes);
    this.navCtrl.push(ChooseWorkTypePage, {
      types: this.selectedWorkTypes,
      maxCount: 1
    });
  }

  // 选中热门/历史搜索
  selectHotSearch(condition) {
    this.keyword = condition;
    this.pageNum = 1;
    this.getUserList();
  }

  goUserDetail(id) {
    this.navCtrl.push(UserInfoPage, {
      workerId: id
    });
  }
}
