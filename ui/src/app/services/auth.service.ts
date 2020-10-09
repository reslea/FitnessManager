import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {BehaviorSubject} from 'rxjs';
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
          localStorage[this.storageKey] = JSON.stringify(userInfo.expires);
        }
      }))
      .subscribe();
  }

  login(username: string, password: string) {
    const jsonBody = JSON.stringify({ username, password });
    this.http.post(`${this.apiUrl}/authenticate`, jsonBody)
      .subscribe((response: TokenResponse) => {
        const token = response.accessToken;
        this.$accessToken.next(token);
        this.$userInfo.next(new UserInfo(jwt_decode(token)));
      });
  }
}
