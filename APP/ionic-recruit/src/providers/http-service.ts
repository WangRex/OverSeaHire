// JavaScript source code
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from "rxjs";
import { api_url } from "./constants";
import { Utils } from "./utils";
import { AlertService } from "./alert.service";

@Injectable()
export class HttpService {

  constructor(
    private httpClient: HttpClient,
    private _Utils: Utils,
    private _AlertService: AlertService
  ) {}

  private buildRequestOptions(paramMap): any {
    let urlparamstr = null;
    if (paramMap != null) {
      for (let key in paramMap) {
        let val = paramMap[key];
        if (urlparamstr === null) {
          urlparamstr = key + "=" + val;
        } else {
          urlparamstr = urlparamstr + "&" + key + "=" + val;
        }
      }
    }
    const paramc = new HttpParams({ fromString: urlparamstr });
    // const headers = new HttpHeaders({ 'Content-Type': 'application/json;application/x-www-form-urlencoded;charset=UTF-8' }).append('token', token);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json;charset=UTF-8;application/x-www-form-urlencoded;charset=UTF-8' });
    let options = { headers: headers, params: paramc };
    // let options = {params: paramc};
    return options;
  }

  public httpGet(reqUrl, paramMap: any = null, showLoading: boolean = true): Observable<any> {
    let options = this.buildRequestOptions(paramMap);
    if (showLoading) {
      this._Utils.showLoading();
    }
    reqUrl = Utils.FormatUrl(reqUrl.startsWith('http') ? reqUrl : api_url + reqUrl);
    return Observable.create(observer => {
      this.httpClient.get(reqUrl, options)
        .subscribe(data => {
          if (showLoading) {
            this._Utils.hideLoading();
          }
          observer.next(data);
        }, error => {
          if (showLoading) {
            this._Utils.hideLoading();
          }
          observer.error(error);
        });
    });
  }

  public httpPost(reqUrl, paramMap: any = null, showLoading: boolean = true, showAlertErrorMsg: boolean = true): Observable<any> {
    if (showLoading) {
      this._Utils.showLoading();
    }
    reqUrl = Utils.FormatUrl(reqUrl.startsWith('http') ? reqUrl : api_url + reqUrl);

    return Observable.create(observer => {
      this.httpClient.post(reqUrl, paramMap)
        .subscribe(data => {
          if (showLoading) {
            this._Utils.hideLoading();
          }
          observer.next(data);
        }, error => {
          if (showLoading) {
            this._Utils.hideLoading();
          }
          if (showAlertErrorMsg) {
            this._AlertService.presentAlert('获取数据异常')
          }
          observer.error(error);
        });
    });
  }

  public httpExportPost(reqUrl, paramMap: any = null) {
    let url = reqUrl;
    if (reqUrl.indexOf('http') === -1) {
      url = api_url + reqUrl;
    }

    let newWin = window.open();
    let formStr: any = '';
    formStr = '<form method="POST" action="' + url + '">';
    for (let key in paramMap) {
      formStr += '<input type="hidden" name="' + key + '" value="' + paramMap[key] + '" />';
    }
    formStr += '</form>';
    newWin.document.body.innerHTML = formStr;
    newWin.document.forms[0].submit();
  }


}
