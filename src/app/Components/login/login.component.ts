import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import { NgIf} from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { PasswordBoxComponent } from '../passwordbox/passwordBox.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterLink,
    PasswordBoxComponent,
    FormsModule, NgIf
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  twoFactorCode: string = '';
  message: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    this.authService.login(this.username, this.password, this.twoFactorCode)
      .subscribe({
        next: (res) => {
          this.message = res.message;
          localStorage.setItem('token', res.username); // Save the token
          this.router.navigate(['/settings']); // Navigate to settings
        },
        error: (err) => {
          this.message = err.error;
        }
      });
  }

}
