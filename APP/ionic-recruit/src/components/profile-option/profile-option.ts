import { Component, Input } from '@angular/core';

@Component({
    selector: 'ion-profile-option',
    templateUrl: 'profile-option.html',
})
export class ProfileOption {
    @Input() label: string = '求职意向';
    @Input() img: string = '';
    constructor() {
    }
}
