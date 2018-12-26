import { Component, Input } from '@angular/core';

@Component({
    selector: 'ion-info-primary',
    templateUrl: 'info-primary.html',
})
export class InfoPrimary {
    @Input() label: string;
    @Input() showImg: boolean = false;
    constructor() {
    }
}
