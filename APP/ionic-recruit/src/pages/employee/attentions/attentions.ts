import { Component, ElementRef } from '@angular/core';
import { NavController } from 'ionic-angular';

@Component({
  selector: 'page-attentions',
  templateUrl: 'attentions.html'
})
export class AttentionsPage {
  headerOffsetH: any;
  showLeftMenu: boolean = true;
  selectedCategory: any;
  categories: Array<any> = [
    {
      id: 0,
      name: '注意事项',
      selected: true
    },
    {
      id: 1,
      name: '办理流程',
      selected: false
    }
  ];
  constructor(
    public navCtrl: NavController,
    private _ElementRef: ElementRef
  ) {

  }

  // 页面第一次加载的时候执行
  ionViewDidLoad() {

  }

  ionViewDidEnter() {
    // 计算header的offset height
    this.headerOffsetH = this._ElementRef.nativeElement.querySelector("ion-header").offsetHeight;
    this.selectedCategory = this.categories[0];
    this.showLeftMenu = true;
  }

  // 页面退出的时候触发-手动隐藏掉左侧菜单，否则总会关闭的时候延迟一下
  ionViewWillLeave() {
    this.showLeftMenu = false;
  }

  // 设置选中子菜单
  setSelectedCrop(category) {
    this.selectedCategory = category;
  }
}
