import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {BehaviorSubject, Observable, throwError} from 'rxjs';
import {map, tap} from 'rxjs/operators';
import jwt_decode from "jwt-decode";
import UserInfo from "../security/userInfo";

interface TokenResponse {
  accessToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = 'https://localhost:5001/api/auth';
  storageKey = 'userInfo';

  readonly $accessToken = new BehaviorSubject<string>(null);
  readonly $userInfo = new BehaviorSubject<UserInfo>(null);

  constructor(readonly http: HttpClient) {
    this.$userInfo
      .pipe(tap(userInfo => {
        if (userInfo) {
          localStorage[this.storageKey] = JSON.stringify(true);
        }
      }))
      .subscribe();
  }

  login(username: string, password: string) {
    const jsonBody = JSON.stringify({ username, password });

    const headers = new HttpHeaders({'Content-Type': 'application/json; charset=utf-8' });
    this.http.post(`${this.apiUrl}/authenticate`, jsonBody, { headers })
      .pipe(tap(this.processToken.bind(this)))
      .subscribe();
  }

  refresh()  {
    if (!localStorage[this.storageKey]) {
      console.log('there is no refresh token, need to login');
      return throwError('there is no refresh token, need to login');
    }
    this.http.get(`${this.apiUrl}/refresh`)
      .pipe(tap(this.processToken.bind(this)))
      .subscribe();
  }

  processToken(response: TokenResponse) {
    const token = response.accessToken;
    console.log(token);
    console.log(`accessSubject`, this.$accessToken);
    console.log(`userSubject`, this.$userInfo);

    const userInfo = new UserInfo(jwt_decode(token));
    this.$accessToken.next(token);
    this.$userInfo.next(userInfo);
  }
}
