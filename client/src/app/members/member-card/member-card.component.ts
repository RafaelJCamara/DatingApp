import { Component, Input } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { LikeService } from 'src/app/_services/like.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent {
  @Input() member: Member | undefined;

  constructor(
    private likeService: LikeService,
    public presenceService: PresenceService
  ) {}

  like(member: Member) {
    if (member.isLikedByCurrentUser) this.dislikeUser(member);
    else this.likeUser(member);
  }

  likeUser(member: Member) {
    this.likeService.likeUser(member);
  }

  dislikeUser(member: Member) {
    this.likeService.dislikeUser(member);
  }
}
