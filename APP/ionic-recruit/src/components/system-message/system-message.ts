import { Component,Input } from '@angular/core';

@Component({
    selector: 'ion-system-message',
    templateUrl: 'system-message.html',
})

export class SystemMessage {
    @Input() title: string = '';
    constructor() {
    }
}
