import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { job_steps, file_url } from "../../../providers/constants";
import { StorageService } from "../../../providers/storage-service";

@Component({
  selector: 'page-current-order-list',
  templateUrl: 'current-order-steps.html'
})
export class CurrentOrderStepsPage {
  fileUrl = file_url;
  steps: Array<any> = [];
  currentStep: number;
  completeSteps: Array<any> = [];
  constructor(
    public navCtrl: NavController,
    private _StorageService: StorageService,
    private _NavParams: NavParams
  ) {}

  // 首次加载时候执行
  ionViewDidLoad() {
    this.currentStep = Number(this._NavParams.get('currentStep'));
    this.steps = this._StorageService.readSession(job_steps);
    this.completeSteps = this._NavParams.get('completeSteps');
  }

  ionViewDidEnter() {}
}
