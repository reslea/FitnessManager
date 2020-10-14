import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import {HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import {AuthTokenInterceptor} from './security/auth-token-interceptor.service';
import { AddCoachComponent } from './coach/add/add-coach.component';
import {Router, RouterModule, Routes} from '@angular/router';
import {PermissionGuard} from './security/permission.guard';
import {PermissionType} from './security/permissionType';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'addCoach',
    component: AddCoachComponent,
    canActivate: [PermissionGuard],
    data: { requiredPermissions: [ PermissionType.AddCoaches ] }
  },
  { path: '**', component: LoginComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AddCoachComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthTokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
