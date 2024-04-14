import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { EmailService } from 'src/app/_services/email.service';

@Component({
  selector: 'app-email-validation',
  templateUrl: './email-validation.component.html',
  styleUrls: ['./email-validation.component.css'],
})
export class EmailValidationComponent implements OnInit {
  emailValidationsuccessful: boolean = false;
  user: User | undefined;

  constructor(
    private route: ActivatedRoute,
    private emailService: EmailService,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.user = user;
      },
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (data) =>
        this.emailService.validateEmail(data['emailValidationCode']).subscribe({
          next: (data) => {
            if (data == null && this.user) {
              this.emailValidationsuccessful = true;
              this.user.emailConfirmed = true;
              this.accountService.setCurrentUser(this.user);
            }
          },
        }),
    });
  }
}
