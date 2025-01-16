import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { PasswordBoxComponent } from '../passwordbox/passwordBox.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterLink,
    NgClass, PasswordBoxComponent,
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  twoFactorCode: string = '';
  message: string = '';

  constructor(private authService: AuthService) {}

  onLogin() {
    this.authService.login(this.username, this.password, this.twoFactorCode)
      .subscribe({
        next: (res) => {
          this.message = 'Login successful';
          // Optionally store token
          // localStorage.setItem('token', res.token);
        },
        error: (err) => {
          this.message = err.error;
        }
      });
  }

}
