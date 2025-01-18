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

  sendOTP(email: string, userId: number) {
    return this.http.post('/api/Auth/send-otp', { email, userId });
  }

  update2FASettings(userId: number, method: string, secretKey: string | null = null): Observable<any> {
    return this.http.put(`${this.baseUrl}/${userId}/2fa`, method);
  }
}
