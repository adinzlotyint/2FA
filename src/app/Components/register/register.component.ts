import { Component } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { FormsModule } from '@angular/forms';
import { NgIf} from '@angular/common';
import { PasswordBoxComponent } from '../passwordbox/passwordBox.component';
import {Router} from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, PasswordBoxComponent, NgIf],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  password = '';
  confirmPassword = '';
  message = '';
  username: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onRegister() {
    if (this.password !== this.confirmPassword) {
      this.message = 'Passwords do not match';
      return;
    }

    this.authService
      .register(this.username, this.password)
      .subscribe({
        next: () => {
          this.message = 'Registration successful';
          this.router.navigate(['']);
        },
        error: (err) => {
          this.message = err.error;
        },
      });
  }
}
