import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';
import { AccountService } from '../_services/account.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map((user) => {
      if (!user) return false;
      if (user.roles.includes('Admin')) {
        return true;
      } else {
        toastr.error('You cannot enter this area');
        return false;
      }
    })
  );
};
