import {Injectable} from '@angular/core';
import { HttpService } from "../../providers/http-service";

@Injectable()
export class LoginService {
  constructor(
    public httpService: HttpService,
  ) {}

  // 登陆或者注册
  // identity(0:工人;1:雇主)
  login(phone, code, identity, showLoading = true) {
    let url = "api/Account/CustomerLoginRegister";
    let postBody = {
      mobile: phone,
      code: code,
      identity: identity
    };
    return this.httpService.httpPost(url, postBody, showLoading).map((res: Response) => res);
  }

  // 获取验证码
  sendVerifyCode(mobile) {
    let postBody = {
      mobile: mobile
    };
    let url = "api/Account/SendCode";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

}
