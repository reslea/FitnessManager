import { Component, OnInit } from '@angular/core';
import {AuthService} from "../services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(readonly authService: AuthService) { }

  username: string = 'user5';
  password: string = '123';

  ngOnInit(): void {
  }

  login(username: string, password: string) {
    this.authService.login(username, password);
  }
}
