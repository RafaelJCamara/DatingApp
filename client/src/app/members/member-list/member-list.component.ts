import { PageChangedEvent, Pagination } from './../../_models/pagination';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

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

  constructor(
    private memberService: MembersService,
    private presenceService: PresenceService
  ) {
    this.memberService.resetUserParams();
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

  pageChanged(event: PageChangedEvent) {
    if (this.userParams) {
      this.userParams.pageNumber = event.pageNumber;
      this.userParams.pageSize = event.pageSize;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
}
