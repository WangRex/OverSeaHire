import { Injectable } from '@angular/core';
import { HttpService } from "../../providers/http-service";
import { page_size } from "../../providers/constants";

@Injectable()
export class BossService {
  constructor(
    public httpService: HttpService,
  ) {}

  // 获取面试中的工友
  getInterviewUsers(userId, requirementId, pageNum) {
    let postBody = {
      UserId: userId,
      RequirementId: requirementId,
      PageNum: pageNum,
      RecordNum: page_size
    };
    let url = "api/Requirement/GetInterviewUsers";
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

  // 获取应聘过需求详情
  getApplyJob(userId, jobId, latitude: any = '', longitude: any = '') {
    let postBody = {
      UserId: userId,
      ApplyJobId: jobId,
      Longitude: longitude,
      Latitude: latitude
    };
    let url = "api/ApplyJob/GetApplyJob";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取收藏列表
  getCollectCustomers(userId, pageNum) {
    let postBody = {
      UserId: userId,
      PageNum: pageNum,
      RecordNum: page_size
    };
    let url = "api/Account/GetCollectCustomers";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 取消收藏用户
  unCollectCustomer(userId, collectId) {
    let postBody = {
      UserId: userId,
      CustomerCollectId: collectId
    };
    let url = "api/Account/UnCollectCustomer";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 收藏用户
  collectCustomer(userId, workerId) {
    let postBody = {
      UserId: userId,
      WorkerId: workerId
    };
    let url = "api/Account/CollectCustomer";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 面试邀请
  inviteWorker(userId, offerId, workerId) {
    let postBody = {
      CustomerId: userId,
      WorkerId: workerId,
      RequirementId: offerId
    };
    let url = "api/Requirement/InviteWorker";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取工友详情
  getCustomerWorkmate(userId, workerId) {
    let postBody = {
      customerId: userId,
      WorkmateId: workerId
    };
    let url = "api/Account/GetCustomerWorkmate";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 获取企业认证
  getCompany(userId) {
    let postBody = {
      UserId: userId
    };
    let url = "api/Account/GetCompany";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 企业认证
  updateCompany(postBody: any) {
    let url = "api/Account/UpdateCompany";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 雇主端推荐人列表
  getRecommendUserList(postBody: any) {
    let url = "api/Requirement/GetRecommendUserList";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 删除岗位需求
  deleteRequirement(userId, reqId) {
    let postBody = {
      UserId: userId,
      RequirementId: reqId
    };
    let url = "api/Requirement/DeleteRequirement";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 初始化岗位需求
  editRequirementInit(userId, reqId) {
    let postBody = {
      UserId: userId,
      RequirementId: reqId
    };
    let url = "api/Requirement/EditRequirementInit";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  // 修改岗位需求
  editRequirement(postBody: any) {
    let url = "api/Requirement/EditRequirement";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 获取需求列表
  getRequirementList(publisher, pageNum) {
    let url = "api/Requirement/GetRequirementList";
    let postBody = {
      PublishUserId: publisher,
      RecordNum: page_size,
      PageNum: pageNum
    };
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 上传附件
  createRequirement(postBody: any) {
    let url = 'api/Requirement/CreateRequirement';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
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

  getPositionTree(userid, showLoading: boolean = true) {
    let postBody = {
      UserId: userid,
    };
    let url = 'api/Requirement/GetPositionTree';
    return this.httpService.httpGet(url, postBody, showLoading).map((res: Response) => res);
  }

  // 获取国家
  getCountries(showLoading = true) {
    let url = "api/Requirement/GetCountries";
    let postBody = {
      UserId: '',
      PageNum: '0',
      RecordNum: '0'
    };
    return this.httpService.httpGet(url, postBody, showLoading).map((res: Response) => res);
  }

  // 获取枚举
  getDics(postBody: any, showLoading: boolean = true) {
    let url = "api/EnumDictionary/GetDics";
    return this.httpService.httpGet(url, postBody, showLoading).map((res: Response) => res);
  }

  // 获取系统配置微信和电话
  getSysConfiguration() {
    let postBody = {
      UserId: ''
    };
    let url = "api/SysConfiguration/GetSysConfiguration";
    return this.httpService.httpGet(url, postBody).map((res: Response) => res);
  }

  //上传图片
  fileUpload(postBody: any) {
    let url = 'api/ImageUpload/UploadBase64';
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }

  // 修改应聘申请
  editApplyJob(postBody: any) {
    let url = "api/ApplyJob/EditApplyJob";
    return this.httpService.httpPost(url, postBody).map((res: Response) => res);
  }
}
