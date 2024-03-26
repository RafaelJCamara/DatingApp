import { Pagination } from './../../_models/pagination';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  pageSizeArray = [5, 10, 25];

  constructor(
    private memberService: MembersService,
    private presenceService: PresenceService
  ) {
    this.userParams = this.memberService.getUserParams();
    this.presenceService.onlineUsers$.subscribe({
      next: (_) => {
        this.loadMembers();
      },
    });
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    if (this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: (response) => {
          if (response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        },
      });
    }
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: PageEvent) {
    if (this.userParams) {
      let changes = false;

      if (this.userParams.pageNumber != event.pageIndex) {
        this.userParams.pageNumber = event.pageIndex;
        changes = true;
      }

      if (this.userParams.pageSize != event.pageSize) {
        this.userParams.pageSize = event.pageSize;
        changes = true;
      }

      if (changes) {
        this.memberService.setUserParams(this.userParams);
        this.loadMembers();
      }
    }
  }
}
