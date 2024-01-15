import { inject } from '@angular/core';
import { AccountService } from './../_services/account.service';
import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const AuthGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map((user) => {
      if (user) return true;
      else {
        toastr.error('You need to be authenticated to access this route.');
        return false;
      }
    })
  );
};
