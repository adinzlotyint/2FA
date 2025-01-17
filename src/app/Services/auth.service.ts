import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:5219/api/auth';
  constructor(private http: HttpClient) {}

  register(username: string, password: string, email: string): Observable<any> {
    const payload = { username, password, email };
    return this.http.post(`${this.baseUrl}/register`, payload);
  }

  login(username: string, password: string, twoFactorCode?: string): Observable<any> {
    const payload = { username, password, twoFactorCode };
    return this.http.post(`${this.baseUrl}/login`, payload);
  }

  logout(): void {
    // For a local scenario, you might just clear localStorage or a session
    localStorage.removeItem('token');
  }
}
