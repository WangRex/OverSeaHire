import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { CustomValidators } from "../../providers/validators";
import { LoginService } from "./login-service";
import { StorageService } from "../../providers/storage-service";
import {boss_flag, user_info, user_info_boss} from "../../providers/constants";

@Component({
  selector: 'page-login',
  templateUrl: 'login.html'
})
export class Login {
  //验证码倒计时
  verifyCode: any = {
    verifyCodeTips: "获取验证码",
    countdown: 60,
    disable: false
  };
  validation: boolean = false;
  phone: any = '';
  verifyNum: any = '';
  constructor(
    public navCtrl: NavController,
    private _CustomValidators: CustomValidators,
    private _LoginService: LoginService,
    private _StorageService: StorageService
  ) {}

  // 校验手机号
  checkPhoneValidation() {
    this.validation = this._CustomValidators.checkPhone(this.phone);
  }

  // 短信倒计时
  setTime() {
    this.verifyCode.disable = true;
    if (this.verifyCode.countdown == 1) {
      this.verifyCode.countdown = 60;
      this.verifyCode.verifyCodeTips = "重新获取";
      this.verifyCode.disable = false;
      return;
    } else {
      this.verifyCode.countdown--;
    }

    this.verifyCode.verifyCodeTips = "重新获取(" + this.verifyCode.countdown + ")";
    setTimeout(() => {
      this.verifyCode.verifyCodeTips = "重新获取(" + this.verifyCode.countdown + ")";
      this.setTime();
    }, 1000);
  }

  // 发送短信
  sendValidationCode() {
    if (this.validation && this.verifyCode.disable == false) {
      // 发送请求获取验证码
      this._LoginService.sendVerifyCode(this.phone).subscribe((data: any) => {
        if (data.Code == 200) {
          this.setTime();
        }
      });
    }
  }

  login() {
    // 0 是工人 1是雇主
    let identity = '0';
    if (this._StorageService.readSession(boss_flag) == true){
      identity = '1';
    }
    if (this.validation && ("" + this.verifyNum).length > 3){
      this._LoginService.login(this.phone, this.verifyNum, identity).subscribe((data: any) => {
        if (data.Code == 200) {
          if (identity == '0') {
            this._StorageService.write(user_info, data.Data);
          } else {
            this._StorageService.write(user_info_boss, data.Data);
          }
          this.navCtrl.pop();
        }
      });
    }
  }
}
