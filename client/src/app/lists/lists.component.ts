import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pageNumber = 0;
  pageSize = 5;
  pageSizeArray = [5, 10, 25];
  pagination: Pagination | undefined;

  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.memberService
      .getLikes(this.predicate, this.pageNumber, this.pageSize)
      .subscribe({
        next: (response) => {
          this.members = response.result;
          this.pagination = response.pagination;
        },
      });
  }

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

    if (changes) this.loadLikes();
  }
}
