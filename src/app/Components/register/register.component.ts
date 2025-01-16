import { Component } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { FormsModule } from '@angular/forms';
import {NgClass, NgIf} from '@angular/common';
import { PasswordBoxComponent } from '../passwordbox/passwordBox.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, PasswordBoxComponent, NgClass, NgIf],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  password = '';
  confirmPassword = '';
  message = '';
  username: string = '';
  email: string = '';

  constructor(private authService: AuthService) {}

  onRegister() {
    if (this.password !== this.confirmPassword) {
      this.message = 'Passwords do not match';
      return;
    }

    this.authService
      .register(this.username, this.password, this.email)
      .subscribe({
        next: (res) => {
          this.message = 'Registration successful';
        },
        error: (err) => {
          this.message = err.error;
        },
      });
  }
}
