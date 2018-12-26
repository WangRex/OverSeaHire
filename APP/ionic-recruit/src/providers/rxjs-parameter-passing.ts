import {Injectable} from '@angular/core';
import {Subject} from 'rxjs/Subject';

@Injectable()
export class ParameterPassingService {

  // 刷新
  private refreshSource = new Subject();
  refreshSource$ = this.refreshSource.asObservable();

  setRefreshOption(options: any) {
    this.refreshSource.next(options);
  }

  // 个人简介
  private personalIntroSource = new Subject();
  personalIntroSource$ = this.personalIntroSource.asObservable();

  setPersonalIntroOption(options: any) {
    this.personalIntroSource.next(options);
  }

  // 个人视频
  private personalVideoSource = new Subject();
  personalVideoSource$ = this.personalVideoSource.asObservable();

  setPersonalVideoSource(options: any) {
    this.personalVideoSource.next(options);
  }

  // 职位描述
  private offerDescriptionSource = new Subject();
  offerDescriptionSource$ = this.offerDescriptionSource.asObservable();

  setOfferDescriptionSource(options: any) {
    this.offerDescriptionSource.next(options);
  }

  // 个人籍贯
  private homeTownSource = new Subject();
  homeTownSource$ = this.homeTownSource.asObservable();

  setHomeTownSource(options: any) {
    this.homeTownSource.next(options);
  }

  // 求职意向-期望职位
  private expectWorkTypeSource = new Subject();
  expectWorkTypeSource$ = this.expectWorkTypeSource.asObservable();

  setExpectWorkTypeSource(options: any) {
    this.expectWorkTypeSource.next(options);
  }

  // 求职意向-期望国家
  private expectCitySource = new Subject();
  expectCitySource$ = this.expectCitySource.asObservable();

  setExpectCitySource(options: any) {
    this.expectCitySource.next(options);
  }
}
