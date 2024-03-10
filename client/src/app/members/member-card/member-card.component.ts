import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent {
  @Input() member: Member | undefined;

  constructor(
    private memberService: MembersService,
    private toastr: ToastrService,
    public presenceService: PresenceService
  ) {}

  like(member: Member) {
    if (member.isLikedByCurrentUser) this.dislikeUser(member);
    else this.likeUser(member);
  }

  likeUser(member: Member) {
    member.isLikedByCurrentUser = true;
    this.memberService.likeUser(member).subscribe({
      next: () => this.toastr.success('You have liked ' + member.knownAs),
    });
  }

  dislikeUser(member: Member) {
    member.isLikedByCurrentUser = false;
    this.memberService.dislikeUser(member).subscribe({
      next: () => this.toastr.warning('You have disliked ' + member.knownAs),
    });
  }
}
