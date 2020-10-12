import { Injectable } from '@angular/core';
import {TrainingType} from '../domain/trainingType';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CoachService {
  apiUrl = 'https://localhost:5003/api/coach';

  constructor(readonly http: HttpClient) { }

  add(firstName: string, lastName: string, trainingType: TrainingType): Promise<any> {
    const jsonBody = JSON.stringify({ firstName, lastName, trainingType });
    const headers = new HttpHeaders({'Content-Type': 'application/json; charset=utf-8'});
    return this.http.post(`${this.apiUrl}`, jsonBody, { headers }).toPromise();
  }
}
