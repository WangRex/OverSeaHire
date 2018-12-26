import { Component,Input } from '@angular/core';

@Component({
    selector: 'ion-flow-message',
    templateUrl: 'flow-message.html',
})

export class FlowMessage {
    @Input() title: string = '';
    constructor() {
    }
}
