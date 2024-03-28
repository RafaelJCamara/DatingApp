import {
  DEFAULT_PAGE_NUMBER,
  DEFAULT_PAGE_SIZE,
} from './constants/paginationConstants';
import { User } from './user';

export class UserParams {
  gender: string;
  minAge = 18;
  maxAge = 99;
  pageNumber = DEFAULT_PAGE_NUMBER;
  pageSize = DEFAULT_PAGE_SIZE;
  orderBy = 'lastActive';

  constructor(user: User) {
    this.gender = user.gender === 'female' ? 'male' : 'female';
  }
}
