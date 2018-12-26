import {Component, ElementRef} from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ParameterPassingService } from "../../providers/rxjs-parameter-passing";
import { search_work_types } from "../../providers/constants";
import { StorageService } from "../../providers/storage-service";
import { AlertService } from "../../providers/alert.service";

@Component({
  selector: 'page-choose-work-type',
  templateUrl: 'choose-work-type.html'
})
export class ChooseWorkTypePage {
  types: Array<any> = [];
  secondTypes: Array<any> = [];
  thirdTypes: Array<any> = [];
  selectedFirstTypes: Array<any> = [];
  selectedSecondTypes: Array<any> = [];
  selectedThirdTypes: Array<any> = [];
  selectedTypes: Array<any> = [];
  headerOffsetH: any;
  showFixedContent: boolean = true;
  showSecondDetail: boolean = false;
  maxCount: number = 3;
  constructor(
    public navCtrl: NavController,
    private _ParameterPassingService: ParameterPassingService,
    private _StorageService: StorageService,
    private _AlertService: AlertService,
    private _NavParams: NavParams,
    private _ElementRef: ElementRef
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.types = this._StorageService.readSession(search_work_types);
    this.selectedThirdTypes = this._NavParams.get('types');
    if (this._NavParams.get('maxCount')) {
      this.maxCount = Number(this._NavParams.get('maxCount'));
    }
    // this.getWorkTypes();
  }

  ionViewDidEnter() {
    this.showFixedContent = true;
    this.headerOffsetH = this._ElementRef.nativeElement.querySelector("ion-header").offsetHeight;
    setTimeout(() => {
      this.headerOffsetH = this._ElementRef.nativeElement.querySelector("ion-header").offsetHeight;
    }, 300);
  }

  // 页面退出的时候触发-手动隐藏掉左侧菜单，否则总会关闭的时候延迟一下
  ionViewWillLeave() {
    this.showFixedContent = false;
  }

  save() {
    if (this.selectedThirdTypes.length == 0) {
      this._AlertService.presentAlert('请选择期望职位');
    } else {
      this._ParameterPassingService.setExpectWorkTypeSource(this.selectedThirdTypes);
      this.navCtrl.pop();
    }
  }

  setSelectedFirstType(option) {
    let index = this.selectedFirstTypes.indexOf(option.Name);
    if (index == -1) {
      this.selectedFirstTypes.push(option.Name);
    }
    this.secondTypes = option.positionTreeVms;
    this.thirdTypes = [];
    this.showSecondDetail = true;
    // else {
    //   this.selectedFirstTypes.splice(index, 1);
    // }
  }

  setSelectedSecondType(option) {
    let index = this.selectedSecondTypes.indexOf(option.Name);
    if (index == -1) {
      this.selectedSecondTypes.push(option.Name);
    }
    this.thirdTypes = option.positionTreeVms;
  }

  setSelectedThirdType(option) {
    let temp = {
      name: option.Name,
      id: option.Id
    };
    let index = this.selectedThirdTypes.indexOf(temp);
    if (index == -1) {
      if (this.selectedThirdTypes.length >= this.maxCount) {
        this._AlertService.presentToast('最多只能选' + this.maxCount +  '个');
      } else {
        this.selectedThirdTypes.push(temp);
      }
    } else {
      this.selectedThirdTypes.splice(index, 1);
    }
  }

  checkSelectedWorkType(option) {
    let flag = false;
    this.selectedThirdTypes.forEach(item => {
      if (item.id == option.Id) {
        flag = true;
      }
    });
    return flag;
  }

  // setSelectedType(option) {
  //     if (option.selected == true) {
  //       this.selectedTypes.splice(this.selectedTypes.indexOf(option), 1);
  //       option.selected = false;
  //     } else {
  //       if (this.selectedTypes.length >= 3) {
  //         this._AlertService.presentAlert('最多只能选三个');
  //       } else {
  //         option.selected = true;
  //         this.selectedTypes.push(option);
  //       }
  //     }
  // }

  // 关闭弹窗
  closeSecondDetail() {
    this.showSecondDetail = false;
  }

  // 清空
  clearTypes() {
    this.selectedFirstTypes = [];
    this.selectedSecondTypes = [];
    this.selectedThirdTypes = [];
    this.showSecondDetail = false;
  }

  // getWorkTypes() {
  //   this.types = this._NavParams.get('types');
  //
  //   // let userInfo: any = this._StorageService.read(user_info);
  //   // let postBody = {
  //   //   CustomerID: userInfo.Id,
  //   //   TableName: 'App_Requirement.EnumPositionType'
  //   // };
  //   // this._EmployeeService.getDics(postBody, false).subscribe((data: any) => {
  //   //   if (data.Code == 200 || data.Code == 0) {
  //   //     this.types = data.Data;
  //   //     types.forEach(type => {
  //   //       this.types.forEach(item => {
  //   //         if (type.ItemValue == item.ItemValue) {
  //   //           item.selected = true;
  //   //           this.selectedTypes.push(item);
  //   //         }
  //   //       });
  //   //     });
  //   //   }
  //   // });
  // }
}
