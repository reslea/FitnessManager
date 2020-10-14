import {Injectable} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {EMPTY, Observable, Subject} from 'rxjs';
import {switchMap, take} from "rxjs/operators";
import validate = WebAssembly.validate;

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {
  requestTimeoutMilliseconds = 30 * 1000;
  isRefreshInProgress = false;
  $refreshSubject = new Subject<boolean>();

  constructor(readonly authService: AuthService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isTokenExpired()) {
      console.log('token is not expired, making request');
      return next.handle(this.injectAuthorizationHeader(req));
    }

    if (req.url.includes('/api/auth/')) {
      console.log('its allowed to use auth API anytime');
      return next.handle(req);
    }

    console.log('token expired');
    if (!this.isRefreshInProgress) {
      console.log('refreshing token');

      this.isRefreshInProgress = true;

      return this.authService.refresh()
        .pipe(switchMap(() => {
          console.log('refresh succeeded');

          this.$refreshSubject.next(true);
          this.isRefreshInProgress = false;
          console.log(`make request to url`);
          return next.handle(this.injectAuthorizationHeader(req));
      }));
    }
    console.log(`await refresh request`);
    return this.$refreshSubject.pipe(
      take(1),
      switchMap(() => next.handle(this.injectAuthorizationHeader(req)))
    );
  }

  isTokenExpired() {
    console.log(`userSubject: ${this.authService.$userInfo.value}`);
    return this.authService.$userInfo.value &&
      this.authService.$userInfo.value.expires &&
      this.authService.$userInfo.value.expires < new Date(Date.now() - this.requestTimeoutMilliseconds);
  }

  injectAuthorizationHeader(req: HttpRequest<any>): HttpRequest<any> {
    console.log(`injecting auth header`, this.authService.$accessToken.value);
    return !this.authService.$accessToken.value
    ? req
    : req.clone({
        headers: new HttpHeaders({
          authorization: `Bearer ${this.authService.$accessToken.value}`
        })
      });
  }
}
