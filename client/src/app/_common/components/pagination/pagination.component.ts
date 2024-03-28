import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import {
  DEFAULT_PAGE_NUMBER,
  DEFAULT_PAGE_SIZE,
} from 'src/app/_models/constants/paginationConstants';
import { PageChangedEvent } from 'src/app/_models/pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
})
export class PaginationComponent {
  @Input() totalItems = 0;
  @Input() pageNumber = DEFAULT_PAGE_NUMBER;
  @Input() pageSize = DEFAULT_PAGE_SIZE;
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;

  @Output() pageChangedEvent = new EventEmitter<PageChangedEvent>();

  pageSizeArray = [5, 10, 25];

  pageChanged(event: PageEvent) {
    let changes = false;

    if (this.pageNumber != event.pageIndex) {
      this.pageNumber = event.pageIndex;
      changes = true;
    }

    if (this.pageSize != event.pageSize) {
      this.pageSize = event.pageSize;
      changes = true;
    }

    if (changes) {
      this.pageChangedEvent.emit({
        pageSize: this.pageSize,
        pageNumber: this.pageNumber,
      });
    }
  }
}
