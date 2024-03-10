import { Injectable } from '@angular/core';
import { MembersService } from './members.service';
import { Member } from '../_models/member';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class LikeService {
  constructor(
    private memberService: MembersService,
    private toastr: ToastrService
  ) {}

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
