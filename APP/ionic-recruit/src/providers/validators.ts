import {Injectable} from "@angular/core";

@Injectable()
export class CustomValidators {

  checkPhone(phone): boolean {
    let ValidatePhone = /^1[3,4,5,7,8]\d{9}$/;
    if (!ValidatePhone.test(phone)) {
      return false;
    } else {
      return true;
    }
  }
}
