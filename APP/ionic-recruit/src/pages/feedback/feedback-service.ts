import {Injectable} from '@angular/core';
import { HttpService } from "../../providers/http-service";

@Injectable()
export class FeedbackService {
  constructor(
    public httpService: HttpService,
  ) {}

  //上传图片
  fileUpload(postBody: any) {
    let url = 'api/ImageUpload/UploadBase64';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 上传附件
  postFile(postBody: any) {
    let url = 'api/ImageUpload/PostFile';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 添加feedback
  createSysFeedback(postBody: any) {
    let url = 'api/Account/CreateSysFeedback';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

}
