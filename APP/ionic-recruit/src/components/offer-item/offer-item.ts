import { Component,Input } from '@angular/core';
import { file_url } from "../../providers/constants";

@Component({
    selector: 'ion-offer-item',
    templateUrl: 'offer-item.html',
})

export class OfferItem {
    fileUrl = file_url;
    @Input() title: string = '';
    @Input() location: string = '';
    @Input() salary: string = '';
    @Input() lowSalary: string = '';
    @Input() highSalary: string = '';
    @Input() unit: string = '年';
    @Input() tags: Array<any> = [];
    @Input() date: string = '';
    @Input() description: string = '';
    @Input() answerCount: string =  '';
    @Input() img: string;

    constructor() {
    }
}
