import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import {HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import {FormsModule} from "@angular/forms";
import {AuthTokenInterceptor} from "./security/auth-token-interceptor.service";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthTokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
