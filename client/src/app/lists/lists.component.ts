import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { PageChangedEvent, Pagination } from '../_models/pagination';
import {
  DEFAULT_PAGE_NUMBER,
  DEFAULT_PAGE_SIZE,
} from '../_models/constants/paginationConstants';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pagination: Pagination | undefined;
  pageNumber = DEFAULT_PAGE_NUMBER;
  pageSize = DEFAULT_PAGE_SIZE;

  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.loadLikes(this.pageNumber, this.pageSize);
  }

  loadLikes(pageNumber = DEFAULT_PAGE_NUMBER, pageSize = DEFAULT_PAGE_SIZE) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.memberService
      .getLikes(this.predicate, pageNumber, pageSize)
      .subscribe({
        next: (response) => {
          this.members = response.result;
          this.pagination = response.pagination;
        },
      });
  }

  pageChanged(event: any) {
    this.loadLikes(event.pageNumber, event.pageSize);
  }

  memberDisliked(event: any) {
    this.loadLikes();
  }
}
