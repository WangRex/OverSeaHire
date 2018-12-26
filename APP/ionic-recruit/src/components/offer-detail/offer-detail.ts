import {Component, Input} from '@angular/core';
import { file_url } from "../../providers/constants";

@Component({
  selector: 'ion-offer-detail',
  templateUrl: 'offer-detail.html',
})

export class OfferDetail {
  fileUrl = file_url;
  @Input() userImg: string = '';
  @Input() userName: string = '';
  @Input() userDes: string = '';
  @Input() showUserInfo: boolean = false;
  @Input() title: string = '';
  @Input() salary: string = '';
  @Input() unit: string = '年';
  @Input() location: string = '';
  @Input() date: string = '';
  @Input() request: string = '';
  @Input() fee: string = '';
  @Input() totalNum: string = '';
  @Input() passedNum: string = '';
  @Input() lowSalary: string = '';
  @Input() highSalary: string = '';
  @Input() img: string = '';

  constructor() {
  }
}
