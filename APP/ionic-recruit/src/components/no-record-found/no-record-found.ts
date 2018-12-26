import { Component, Input } from '@angular/core';

@Component({
    selector: 'ion-no-record-found',
    templateUrl: 'no-record-found.html',
})
export class NoRecordFound {
    @Input() label: string = '没有发现记录';
    @Input() img: string = 'assets/imgs/no-record.png';
    constructor() {
    }
}
