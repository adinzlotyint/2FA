import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { PasswordBoxComponent } from '../passwordbox/passwordBox.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterLink,
    PasswordBoxComponent,
    FormsModule,
    NgIf
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  // Dane logowania
  username: string = '';
  password: string = '';
  twoFactorCode: string = '';
  message: string = '';
  userId: number | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    // Prześlij nazwę użytkownika, hasło i kod 2FA (jeśli podano)
    this.authService.login(this.username, this.password, this.twoFactorCode)
      .subscribe({
        next: (res) => {
          console.log('Response from server:', res); // Logowanie odpowiedzi

          if (res.requires2FA) {
            if (!this.twoFactorCode) {
              // Użytkownik nie podał kodu 2FA, ale jest wymagany
              this.message = res.message || 'Two-factor authentication is required. Proszę wprowadzić kod TOTP.';
            } else {
              // Kod 2FA jest wymagany i został podany, ale nadal wymaga weryfikacji
              this.message = res.message || 'Invalid TOTP code. Please try again.';
            }
          } else {
            // Udane logowanie bez 2FA lub z prawidłowym 2FA
            this.authService.setUserId(res.userId);
            localStorage.setItem('token', res.token); // Zapisz token w bezpieczny sposób
            this.router.navigate(['/settings']); // Przekieruj do ustawień lub dashboardu
          }
        },
        error: (err) => {
          // Obsłuż błędy logowania
          this.message = err.error?.message || 'Wystąpił błąd podczas logowania.';
        }
      });
  }
}
