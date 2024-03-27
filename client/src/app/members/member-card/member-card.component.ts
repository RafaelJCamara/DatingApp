import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
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
  @Output() memberDislikedEvent = new EventEmitter<boolean>();

  constructor(
    private likeService: LikeService,
    public presenceService: PresenceService,
    private toastr: ToastrService
  ) {}

  like(member: Member) {
    if (member.isLikedByCurrentUser) this.dislikeUser(member);
    else this.likeUser(member);
  }

  likeUser(member: Member) {
    this.likeService.likeUser(member).subscribe({
      next: () => this.toastr.success('You have liked ' + member.knownAs),
    });
  }

  dislikeUser(member: Member) {
    this.likeService.dislikeUser(member).subscribe({
      next: () => this.toastr.warning('You have disliked ' + member.knownAs),
      complete: () => this.memberDislikedEvent.emit(true),
    });
  }
}
