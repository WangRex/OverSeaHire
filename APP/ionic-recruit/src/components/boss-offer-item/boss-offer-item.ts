import { Component,Input } from '@angular/core';
import { file_url } from "../../providers/constants";

@Component({
    selector: 'ion-boss-offer-item',
    templateUrl: 'boss-offer-item.html',
})

export class BossOfferItem {
    fileUrl = file_url;
    @Input() title: string = '';
    @Input() location: string = '';
    @Input() tags: Array<any> = [];
    @Input() img: string;
    @Input() viewCount: string = '';
    @Input() applyCount: string = '';
    @Input() interviewCount: string = '';
    @Input() passedCount: string = '';
    @Input() status: string = '';

    constructor() {
    }
}
