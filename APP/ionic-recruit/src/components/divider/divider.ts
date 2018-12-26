import { Component,Input } from '@angular/core';

@Component({
    selector: 'ion-title-divider',
    templateUrl: 'divider.html',
})

export class Divider {
    @Input() title: string = '';
    constructor() {
    }
}
