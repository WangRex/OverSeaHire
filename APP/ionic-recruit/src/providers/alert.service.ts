import { Injectable } from '@angular/core';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

@Injectable()
export class AlertService {
  constructor(public alertCtrl: AlertController, private toastCtrl: ToastController) { }

  presentAlert(message: string, title?: string) {
    const alert = this.alertCtrl.create(
      {
        title,
        subTitle: message,
        buttons: [
          {
            text: '确认'
          }
        ]
      });

    return alert.present();
  }



  presentErrorAlert(message: string) {
    return this.presentAlert('An error has occurred.', message);
  }

  presentAlertWithCallback(title: string, message: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      const confirm = this.alertCtrl.create({
        title,
        message,
        buttons: [{
          text: '返回',
          role: 'cancel',
          handler: () => {
            confirm.dismiss().then(() => resolve(false));
            return false;
          }
        }, {
          text: '确认',
          handler: () => {
            confirm.dismiss().then(() => resolve(true));
            return false;
          }
        }]
      });

      return confirm.present();
    });
  }

  presentToast(message: string, duration: number = 1000, position: string = 'top') {
    let toast = this.toastCtrl.create({
      message: message,
      duration: duration,
      position: position
    });
    return toast.present();
  }
}
