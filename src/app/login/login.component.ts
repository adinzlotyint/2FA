import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterLink,
    NgClass,
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  passwordValue = '';
  visible = false;

  getPasswordIconClass(): string {
    if (this.passwordValue.trim() === '') {
      return 'bx bxs-lock-alt';
    } else {
      return this.visible ? 'bx bxs-lock-alt' : 'bx bx-show';
    }
  }

  toggleVisibility(): void {
    if (this.passwordValue.trim() !== '') {
      this.visible = !this.visible;
    }
  }
}
