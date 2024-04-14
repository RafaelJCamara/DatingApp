import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null); //this is here to that other services can check if the user is logged in or not (and the type of observable is this because it allows us to set an initial value)
  currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private http: HttpClient,
    private presenceService: PresenceService,
    private toastr: ToastrService
  ) {}

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
          if (!user.emailConfirmed)
            this.toastr.warning(
              'Please make sure to validate your email, otherwise your account will be blocked.',
              'Email not validated'
            );
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? (user.roles = roles) : user.roles.push(roles); //we do this because if a user has only one rule, this comes as a string in the JWT, but if it has many it comes as a string array
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    this.presenceService.createHubConnection(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1])); //retrieves the middle part of he JWT (which contains the main information)
  }
}
