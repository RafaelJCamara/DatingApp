import { Injectable } from '@angular/core';
import { MembersService } from './members.service';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root',
})
export class LikeService {
  constructor(private memberService: MembersService) {}

  likeUser(member: Member) {
    member.isLikedByCurrentUser = true;
    return this.memberService.likeUser(member);
  }

  dislikeUser(member: Member) {
    member.isLikedByCurrentUser = false;
    return this.memberService.dislikeUser(member);
  }
}
