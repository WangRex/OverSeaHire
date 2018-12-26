import { Component,Input } from '@angular/core';

@Component({
    selector: 'ion-single-select',
    templateUrl: 'SingleSelectComponent.html',
})
export class SingleSelectComponent {
    @Input() imageSrc: string = 'assets/images/icon/tractor.png';
    @Input() label: string = '';
    @Input() selected: boolean = false;
    @Input() showRadio: boolean = true;
    constructor() {
    }
}
