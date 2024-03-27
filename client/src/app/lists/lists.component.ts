import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pagination: Pagination | undefined;
  pageNumber = 0;
  pageSize = 5;

  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.loadLikes(this.pageNumber, this.pageSize);
  }

  loadLikes(pageNumber = 0, pageSize = 5) {
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
}
