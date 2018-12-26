import { Component, ViewChild, ElementRef  } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Content } from "ionic-angular";
import { EmployeeService } from "../employee-service";
import { AlertService } from "../../../providers/alert.service";
import { StorageService } from "../../../providers/storage-service";
import { search_history, search_work_types } from "../../../providers/constants";
import { OfferDetailPage } from "../offer-detail/offer-detail";
import { ChooseWorkTypePage } from "../../choose-work-type/choose-work-type";
import { ParameterPassingService } from "../../../providers/rxjs-parameter-passing";

@Component({
  selector: 'page-search-offer',
  templateUrl: 'search-offer.html'
})
export class SearchOfferPage {
  @ViewChild(Content) content: Content;
  headerOffsetH: any;
  keyword: string = '';
  searchAllFlag: boolean = true;
  showFixedContent: boolean = true;
  showWorkType: boolean = false;
  data: Array<any> = [];
  loadMore: boolean = false;
  pageNum: number = 1;
  hotSearch: Array<any> = [];
  searched: boolean = false;
  searchHistory: Array<any> = [];
  types: Array<any> = [];
  selectedTypes: string = '';
  showLocations: boolean = false;
  selectedLocation: string = '';
  locations: Array<any> = [];
  showSalaries: boolean = false;
  selectedSalary: string = '';
  salaries: Array<any> = [];
  showProvinces: boolean = false;
  selectedProvince: string = '';
  provinces: Array<any> = [];
  sorts: Array<any> = [];
  isRecommend: string = '1';
  isLatest: string = '';
  isHot: string = '';
  isSalary: string = '';
  showSort: boolean = false;
  workTypes: Array<any> = [];
  workTypeSubscription: any;
  selectedWorkTypes: Array<any> = [];

  constructor(
    public navCtrl: NavController,
    private _ElementRef: ElementRef,
    private _EmployeeService: EmployeeService,
    private _AlertService: AlertService,
    private _StorageService: StorageService,
    private _ParameterPassingService: ParameterPassingService
  ) {
    this.sorts = [{
      value: 'isRecommend',
      text:  '推荐优先',
      selected: true
    },{
      value: 'isLatest',
      text:  '最新优先',
      selected: false
    },{
      value: 'isHot',
      text:  '最热优先',
      selected: false
    },{
      value: 'isSalary',
      text:  '高薪优先',
      selected: false
    }]
  }

  ionViewDidLoad() {
    this.getHotSearch();
    if (this._StorageService.read(search_history)) {
      this.searchHistory = this._StorageService.read(search_history);
    }

    // 订阅期望职位
    this.workTypeSubscription = this._ParameterPassingService.expectWorkTypeSource$.subscribe(
      (data: any) => {
        this.selectedWorkTypes = data;
        this.searchRequirementList();
      });
  }

  ionViewDidEnter() {
    this.showFixedContent = true;
  }

  // 页面退出的时候触发-手动隐藏掉左侧菜单，否则总会关闭的时候延迟一下
  ionViewWillLeave() {
    this.showFixedContent = false;
  }

  // convertFilterText(str) {
  //   if(str.length > 4) {
  //     let result = str.substring(0, 4) + '...';
  //     return result;
  //   } else {
  //     return str;
  //   }
  // }

  toggleWorkType() {
    this.showWorkType = !this.showWorkType;
    if (this.showWorkType) {
      this.showLocations = false;
      this.showSalaries = false;
      this.showProvinces = false;
      this.showSort = false;
    }
  }

  toggleLocations() {
    this.showLocations = !this.showLocations;
    if (this.showLocations) {
      this.showWorkType = false;
      this.showSalaries = false;
      this.showProvinces = false;
      this.showSort = false;
    }
  }

  toggleSalaries() {
    this.showSalaries = !this.showSalaries;
    if (this.showSalaries) {
      this.showWorkType = false;
      this.showLocations = false;
      this.showProvinces = false;
      this.showSort = false;
    }
  }

  toggleProvinces() {
    this.showProvinces = !this.showProvinces;
    if (this.showProvinces) {
      this.showWorkType = false;
      this.showLocations = false;
      this.showSalaries = false;
      this.showSort = false;
    }
  }

  toggleSort() {
    this.showSort = !this.showSort;
    if (this.showSort) {
      this.showWorkType = false;
      this.showLocations = false;
      this.showSalaries = false;
      this.showProvinces = false;
    }
  }

  searchRequirementList(event?) {
    if (this.keyword.length < 2) {
      this._AlertService.presentToast('职位/公司关键字长度不能少于2个字', 2000, 'bottom');
      return;
    }

    // 如果搜索历史里没有过关键字直接添加到数组最前面
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
    this._StorageService.write(search_history, this.searchHistory);

    let tempArr = [];
    this.selectedWorkTypes.forEach(item => {
      tempArr.push(item.id);
    });

    let postBody = {
      QueryStr: this.keyword,
      PK_App_Position_Name: tempArr.join(','),
      Country: this.selectedLocation,
      SalaryMin: this.selectedSalary,
      TransactProvince: this.selectedProvince,
      IsRecommend: this.isRecommend,
      IsLatest: this.isLatest,
      IsHighSalary: this.isSalary,
      IsHot: this.isHot,
      PageNum: 0,
      RecordNum: 0
    };

    this._EmployeeService.searchRequirementList(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        this.searched = true;
        this.workTypes = data.Data.positionTreeVms;
        if (this.pageNum == 1) {
          this.data = data.Data.requirementVms;
        } else {
          this.data = this.data.concat(data.Data.requirementVms);
        }

        this.clearFilterContent();
        let tempLocation = [];
        let tempType = [];
        let tempSalary = [];
        let tempProvince = [];
        this.data.forEach(item => {
          if (tempType.indexOf(item.PositionType) == -1) {
            tempType.push(item.PositionType);
            this.types.push({
              value: item.PositionType,
              selected: this.selectedTypes.indexOf(item.PositionType) == -1 ? false : true,
              text: item.PositionType
            });
          }

          if (item.CountryName != ''  && tempLocation.indexOf(item.CountryName) == -1) {
            tempLocation.push(item.CountryName);
            this.locations.push({
              value: item.CountryName,
              selected: false,
              text: item.CountryName
            });
          }

          if (item.TransactProvinceName != '' && tempProvince.indexOf(item.TransactProvinceName) == -1) {
            tempProvince.push(item.TransactProvinceName);
            this.provinces.push({
              value: item.TransactProvinceCode,
              selected: false,
              text: item.TransactProvinceName
            });
          }

          let minSalary = Number(item.SalaryLow.replace('万', ''));
          if (tempSalary.indexOf(minSalary) == -1) {
            tempSalary.push(minSalary);
          }
        });
        tempSalary = tempSalary.sort();
        tempSalary.forEach(option => {
          this.salaries.push({
            value: option * 10000,
            selected: false,
            text: option + '万以上',
          })
        });

        this.loadMore = this.data.length < data.DataCount ? true : false;
      }
      this.content.resize();
      if (this.data.length > 0) {
        setTimeout(() => {
          this.headerOffsetH = this._ElementRef.nativeElement.querySelector(".search-header").offsetHeight;
        }, 100);
      }

      if (event) {
        event.complete();
      }
    });
  }

  // 下拉刷新
  doInfinite(infiniteScroll) {
    this.pageNum++;
    this.searchRequirementList(infiniteScroll);
  }

  // 获取热搜列表
  getHotSearch() {
    let postBody = {
      PageNum: 1,
      RecordNum: 15,
      UserId: ''
    };
    this._EmployeeService.getHotSearch(postBody).subscribe((data: any) => {
      if (data.Code == 200) {
        this.hotSearch = data.Data;
      }
    });
  }

  // 选中热门/历史搜索
  selectHotSearch(condition) {
    this.keyword = condition;
    this.pageNum = 1;
    this.searchRequirementList();
  }

  // 清除历史记录
  clearSearchHistory() {
    this.searchHistory = [];
    this._StorageService.write(search_history, []);
  }

  clearFilterContent() {
    this.types = [{
      value: '',
      text: '不限',
      selected: false
    }];
    this.locations = [{
      value: '',
      text: '不限',
      selected: false
    }];
    this.salaries = [{
      value: '',
      text: '不限',
      selected: false
    }];
    this.provinces = [{
      value: '',
      text: '不限',
      selected: false
    }];
  }

  // 设置选中的筛选提交
  setSelectedOption(option, type) {
    if (type != 'sort') {
      option.selected = !option.selected;
    } else {
      option.selected = true;
    }

    if (type == 'types') {
      if (option.selected) {
        if (option.value == '') {
          this.types.forEach(item => {
            if (item.value != option.value) {
              item.selected  = false;
            }
          });
        } else {
          this.types.forEach(item => {
            if (item.value == '') {
              item.selected  = false;
            }
          });
        }
      }
    }

    if (type == 'locations') {
      this.locations.forEach(item => {
        if (item.value != option.value) {
          item.selected  = false;
        }
      })
    }

    if (type == 'salaries') {
      this.salaries.forEach(item => {
        if (item.value != option.value) {
          item.selected  = false;
        }
      })
    }

    if (type == 'provinces') {
      this.provinces.forEach(item => {
        if (item.value != option.value) {
          item.selected  = false;
        }
      })
    }

    if (type == 'sort') {
      this.sorts.forEach(item => {
        if (item.value != option.value) {
          item.selected  = false;
        }
      })
    }
  }

  confirmSelectedTypes() {
    this.showWorkType = false;
    let tempArr =  [];
    this.types.forEach(item => {
      if (item.selected) {
        tempArr.push(item.value);
      }
    });
    this.selectedTypes = tempArr.join(',');
    this.searchRequirementList();
  }

  confirmSelectedLocation() {
    this.selectedLocation = '';
    this.showLocations = false;
    this.locations.forEach(item => {
      if (item.selected) {
       this.selectedLocation = item.value;
      }
    });
    this.searchRequirementList();
  }

  confirmSelectedSalary() {
    this.selectedSalary = '';
    this.showSalaries = false;
    this.salaries.forEach(item => {
      if (item.selected) {
        this.selectedSalary = item.value;
      }
    });
    this.searchRequirementList();
  }

  confirmSelectedProvince() {
    this.selectedProvince = '';
    this.showProvinces = false;
    this.provinces.forEach(item => {
      if (item.selected) {
        this.selectedProvince = item.value;
      }
    });
    this.searchRequirementList();
  }

  confirmSelectedSort() {
    this.showSort = false;
    this.sorts.forEach(item => {
      if (item.selected) {
        if (item.value == 'isRecommend') {
          this.isRecommend = '1';
          this.isLatest = '';
          this.isHot = '';
          this.isSalary = '';
        }

        if (item.value == 'isLatest') {
          this.isRecommend = '';
          this.isLatest = '1';
          this.isHot = '';
          this.isSalary = '';
        }

        if (item.value == 'isHot') {
          this.isRecommend = '';
          this.isLatest = '';
          this.isHot = '1';
          this.isSalary = '';
        }

        if (item.value == 'isSalary') {
          this.isRecommend = '';
          this.isLatest = '';
          this.isHot = '';
          this.isSalary = '1';
        }
      }
    });
    this.searchRequirementList();
  }

  goOfferDetailPage() {
    this.navCtrl.push(OfferDetailPage);
  }

  goChooseWorkTypePage() {
    this._StorageService.writeSession(search_work_types, this.workTypes);
    this.navCtrl.push(ChooseWorkTypePage, {
      types: this.selectedWorkTypes
    });
  }
}
