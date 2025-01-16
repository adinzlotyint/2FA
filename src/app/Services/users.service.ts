import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private baseUrl = 'http://localhost:5219/api/users'; // Your ASP.NET Core endpoint

  constructor(private http: HttpClient) {}

  get2FASettings(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${userId}/2fa`);
  }

  update2FASettings(userId: number, method: string, secretKey: string): Observable<any> {
    const payload = { method, secretKey };
    return this.http.put(`${this.baseUrl}/${userId}/2fa`, payload);
  }
}
