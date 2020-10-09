import {Injectable} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from "rxjs";

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {

  constructor(readonly authService: AuthService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const reqWithAuth = req.clone({
      headers: new HttpHeaders({
        authorization: `Bearer ${this.authService.$accessToken.value}`,
        'Content-Type': 'application/json; charset=utf-8',
      })
    });

    return next.handle(reqWithAuth);
  }
}
