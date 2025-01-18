import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../Services/users.service';
import { Router } from '@angular/router';
import {FormsModule} from '@angular/forms';
import {NgForOf, NgIf} from '@angular/common';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  imports: [
    FormsModule,
    NgForOf,
    NgIf
  ],
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  userId: number = 1; // Example - get the actual userId from your login context
  method: string = '';
  secretKey: string = '';
  message: string = '';
  authMethods: string[] = [
    'TOTP',
    'EmailOTP'
  ];

  constructor(private usersService: UsersService, private router: Router) {}

  ngOnInit(): void {
    this.usersService.get2FASettings(this.userId)
      .subscribe({
        next: (res) => {
          this.method = res.method;
          this.secretKey = res.secretKey;
        },
        error: (err) => {
          this.message = err.error;
        }
      });
  }

  onMethodChange(): void {
      this.secretKey = '';
  }

  onSaveSettings() {
    this.usersService.update2FASettings(this.userId, this.method, this.secretKey)
      .subscribe({
        next: (res) => console.log('TOTP settings updated:', res),
        error: (err) => console.error('Error updating TOTP settings:', err)
      });
  }

  onLogout() {
    sessionStorage.removeItem('token'); // Clear the session token
    this.router.navigate(['']); // Navigate to login page or home
  }
}
