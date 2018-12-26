import { Injectable } from '@angular/core';
import { Loading, LoadingController, Platform } from 'ionic-angular';
/**
 * Utils类存放和业务无关的公共方法
 * @description
 */
@Injectable()
export class Utils {
  private loading: Loading;
  private loadingIsOpen: boolean = false;
  constructor(
    public loadingCtrl: LoadingController,
    private platform: Platform,
  ) {}


  /**
   * 是否真机环境
   */
  isMobile(): boolean {
    return this.platform.is('mobile') && !this.platform.is('mobileweb');
  }

  /**
   * 是否android真机环境
   */
  isAndroid(): boolean {
    return this.isMobile() && this.platform.is('android');
  }

  /**
   * 是否ios真机环境
   */
  isIos(): boolean {
    return this.isMobile() && (this.platform.is('ios') || this.platform.is('ipad') || this.platform.is('iphone'));
  }


  /**
   * 统一调用此方法显示loading
   * @param content 显示的内容
   */
  showLoading(content: string = ''): void {
    if (!this.loadingIsOpen) {
      this.loadingIsOpen = true;
      this.loading = this.loadingCtrl.create({
        content: content
      });
      this.loading.present();
      setTimeout(() => {//最长显示15秒
        this.loadingIsOpen && this.loading.dismiss();
        this.loadingIsOpen = false;
      }, 15000);
    }
  };

  /**
   * 关闭loading
   */
  hideLoading(): void {
    this.loadingIsOpen && this.loading.dismiss();
    this.loadingIsOpen = false;
  };

  /**
   * 把url中的双斜杠替换为单斜杠
   * 如:http://localhost:8080//api//demo.替换后http://localhost:8080/api/demo
   * @param url
   * @returns {string}
   */
  static FormatUrl(url: string = ''): string {
    let index = 0;
    if (url.startsWith('http')) {
      index = 7
    }
    return url.substring(0, index) + url.substring(index).replace(/\/\/*/g, '/');
  }
}
