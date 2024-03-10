import { UserParams } from './../_models/userParams';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { UpdateListWithObject } from '../utils/utils';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  pagintedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  userParams: UserParams | undefined;
  user: User | undefined;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.user = user;
          this.userParams = new UserParams(user);
        }
      },
    });
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(userParams: UserParams) {
    this.userParams = userParams;
  }

  resetUserParams() {
    if (this.user) {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
  }

  getMembers(userParams: UserParams) {
    let params = getPaginationHeaders(
      userParams.pageNumber,
      userParams.pageSize
    );

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return getPaginatedResult<Member[]>(
      this.baseUrl + 'users',
      params,
      this.http
    ).pipe(
      map((response) => {
        return response;
      })
    );
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http
      .put(this.baseUrl + 'users', member)
      .pipe(map(() => UpdateListWithObject(this.members, member)));
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  likeUser(member: Member) {
    return this.http
      .post(this.baseUrl + 'likes/' + member.userName, {})
      .pipe(map(() => UpdateListWithObject(this.members, member)));
  }

  dislikeUser(member: Member) {
    return this.http
      .post(this.baseUrl + 'likes/dislike/' + member.userName, {})
      .pipe(map(() => UpdateListWithObject(this.members, member)));
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number) {
    let params = getPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate);

    return getPaginatedResult<Member[]>(
      this.baseUrl + 'likes',
      params,
      this.http
    );
  }
}
