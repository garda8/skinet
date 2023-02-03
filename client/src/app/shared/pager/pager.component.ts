import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss'],
})
export class PagerComponent {

  @Input() pageSize?: number;
  @Input() totalCount?: number;
  @Output() pageChanged = new EventEmitter<number>();


  onPagerChanged(event: any) {
    //console.log("trying to send number: " +event.page)
    this.pageChanged.emit(event.page);
  }
}
