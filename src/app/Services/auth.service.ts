import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:5219/api/auth';
  constructor(private http: HttpClient) {}

  register(username: string, password: string): Observable<any> {
    const payload = { username, password};
    return this.http.post(`${this.baseUrl}/register`, payload);
  }

  login(username: string, password: string, twoFactorCode?: string): Observable<any> {
    const payload = { username, password, twoFactorCode };
    return this.http.post(`${this.baseUrl}/login`, payload);
  }

  logout(): void {
    localStorage.removeItem('userId');
  }

  setUserId(userId: number): void {
    localStorage.setItem('userId', userId.toString());
  }

  getUserId(): number | null {
    const userId = localStorage.getItem('userId');
    return userId ? parseInt(userId, 10) : null;
  }
}
