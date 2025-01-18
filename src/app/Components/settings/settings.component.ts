// src/app/Components/settings/settings.component.ts

import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../Services/users.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgForOf, NgIf } from '@angular/common';
import { AuthService } from '../../Services/auth.service';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf
  ],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [Clipboard] // Provide Clipboard service
})
export class SettingsComponent implements OnInit {
  userId: number | null = null;
  method: string = 'TOTP'; // Default to TOTP
  secretKey: string = '';
  message: string = '';
  authMethods: string[] = [
    'TOTP'
  ];

  constructor(
    private usersService: UsersService,
    private router: Router,
    private authService: AuthService,
    private clipboard: Clipboard // Inject Clipboard service
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    if (this.userId === null) {
      this.message = 'User not logged in.';
      this.router.navigate(['']); // Redirect to login
      return;
    }

    this.usersService.get2FASettings(this.userId)
      .subscribe({
        next: (res) => {
          this.method = res.method;
          this.secretKey = res.secretKey;
        },
        error: (err) => {
          this.message = err.error?.message || 'Error fetching 2FA settings.';
        }
      });
  }

  onMethodChange(): void {
    if (this.method === 'TOTP') {
      // Enable TOTP: Send update request without secretKey to let backend generate it
      this.usersService.update2FASettings(this.userId!, this.method, '')
        .subscribe({
          next: (res) => {
            this.secretKey = res.secretKey; // Update secretKey from response
            this.message = 'TOTP has been enabled. Secret key generated.';
          },
          error: (err) => {
            this.message = err.error?.message || 'Error enabling TOTP.';
          }
        });
    } else {
      // Handle other methods if any in future
      this.secretKey = '';
    }
  }

  onSaveSettings() {
    // Since TOTP is enabled via onMethodChange, this can be used for other settings
    // Or remove if not needed
  }

  onLogout() {
    this.authService.logout(); // Clear userId and token
    this.router.navigate(['']); // Navigate to login page or home
  }

  /**
   * Copies the secret key to the clipboard.
   */
  copySecretKey() {
    if (this.secretKey) {
      this.clipboard.copy(this.secretKey);
      this.message = 'Sekretny klucz został skopiowany do schowka.';
      setTimeout(() => this.message = '', 3000); // Clear message after 3 seconds
    }
  }
}
