import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EmailService {
  baseUrl = environment.emailApiUrl;

  constructor(private http: HttpClient) {}

  validateEmail(emailValidationCode: string) {
    return this.http.post<boolean>(
      this.baseUrl + `emails/validate/${emailValidationCode}`,
      {}
    );
  }
}
