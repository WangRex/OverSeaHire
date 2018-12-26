import { Component,Input } from '@angular/core';
import { file_url } from "../../providers/constants";

@Component({
    selector: 'ion-steps',
    templateUrl: 'steps.html',
})

export class Steps {
    fileUrl = file_url;
    @Input() title: string = '';
    @Input() index: string = '';
    @Input() icon: string = '';
    @Input() description: string = '';
    @Input() showIndex: boolean = true;
    constructor() {
    }
}
