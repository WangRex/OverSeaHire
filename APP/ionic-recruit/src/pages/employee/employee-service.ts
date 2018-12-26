import {Injectable} from '@angular/core';
import { HttpService } from "../../providers/http-service";
import { page_size } from "../../providers/constants";

@Injectable()
export class EmployeeService {
  constructor(
    public httpService: HttpService,
  ) {}

  // 获取需求列表
  getRequirementList(recommend, latest, highSalary, hot, pageNum) {
    let url = "api/Requirement/GetRequirementList";
    let postBody = {
      IsRecommend: recommend,
      IsLatest: latest,
      IsHighSalary: highSalary,
      IsHot: hot,
      RecordNum: page_size,
      PageNum: pageNum
    };
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取需求列表
  searchRequirementList(postBody: any) {
    let url = "api/Requirement/GetRequirementList";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取收藏需求列表
  getRequirementCollections(postBody: any) {
    let url = "api/Requirement/GetRequirementCollections";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取应聘过需求列表
  getRequirementApplieds(postBody: any) {
    let url = "api/Requirement/GetRequirementApplieds";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取应聘过需求详情
  getApplyJob(userid, jobId, latitude: any = '', longitude: any = '') {
    let postBody = {
      UserId: userid,
      ApplyJobId: jobId,
      Longitude: longitude,
      Latitude: latitude
    };
    let url = "api/ApplyJob/GetApplyJob";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  getPositionTree(userid, showLoading: boolean = true) {
    let postBody = {
      UserId: userid,
    };
    let url = 'api/Requirement/GetPositionTree';
    return this.httpService.httpGet(url, postBody, showLoading).map((res: Response) => res);
  }

  // 修改应聘申请
  editApplyJob(postBody: any) {
    let url = "api/ApplyJob/EditApplyJob";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 编辑个人信息
  updateCustomerInfo(postBody: any) {
    let url = 'api/Account/UpdateCustomerInfo';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取工友列表
  getMyWorkerList(userid, pageNum) {
    let url = "api/Account/GetCustomerWorkmates";
    let postBody = {
      customerId: userid,
      RecordNum: page_size,
      PageNum: pageNum
    };
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  getMyWorkerDetail(userid, workerId) {
    let url = 'api/Account/GetCustomerWorkmate';
    let postBody = {
      customerId: userid,
      WorkmateId: workerId,
    };
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 添加编辑用户工友
  createEditCustomerWorkmate(postBody: any) {
    let url = 'api/Account/CreateEditCustomerWorkmate';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 编辑求职意向
  updateCustomerJobInt(postBody: any) {
    let url = 'api/Account/UpdateCustomerJobInt';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 提交应聘申请
  createApplyJob(postBody: any) {
    let url = 'api/ApplyJob/CreateApplyJob';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取热搜列表
  getHotSearch(postBody: any) {
    let url = "api/Requirement/GetHotSearches";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取系统配置微信和电话
  getSysConfiguration() {
    let postBody = {
      UserId: ''
    };
    let url = "api/SysConfiguration/GetSysConfiguration";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取需求详情
  getRequirementDetail(id, userID = '') {
    let postBody = {
      UserId: userID,
      RequirementId: id
    };
    let url = "api/Requirement/GetRequirement";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 收藏需求
  collectRequirement(userId, requirementId) {
    let postBody = {
      UserId: userId,
      RequirementId: requirementId
    };
    let url = "api/Requirement/CollectRequirement";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 收藏需求
  unCollectRequirement(userId, collectId) {
    let postBody = {
      UserId: userId,
      RequirementCollectId: collectId
    };
    let url = "api/Requirement/UnCollectRequirement";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取用户信息
  getCustomerInfo(id) {
    let postBody = {
      customerId: id
    };
    let url = "api/Account/GetCustomerInfo";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取求职意向
  getCustomerIntension(id) {
    let postBody = {
      customerId: id
    };
    let url = "api/Account/GetCustomerIntension";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

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

  // 获取枚举
  getDics(postBody: any, showLoading: boolean = true) {
    let url = "api/EnumDictionary/GetDics";
    return this.httpService.httpGet(url, postBody, showLoading).map((res: Response) => res);
  }

  // 获取国家
  getCountries() {
    let url = "api/Requirement/GetCountries";
    let postBody = {
      UserId: '',
      PageNum: '0',
      RecordNum: '0'
    };
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取应聘申请流程
  getApplyStep(showLoading = false) {
    let postBody = {
      UserId: ''
    };
    let url = "api/ApplyJob/GetApplyStep";
    return this.httpService.httpGet(url, postBody, showLoading).map((res: Response) => res);
  }

  // 获取应聘申请流程
  getApplyJobs(userid, pageNum) {
    let postBody = {
      UserId: userid,
      CustomerId: userid,
      PageNum: pageNum,
      RecordNum: page_size
    };
    let url = "api/ApplyJob/GetApplyJobs";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }
}
