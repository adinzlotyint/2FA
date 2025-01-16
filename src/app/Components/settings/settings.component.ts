import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../Services/users.service';

@Component({
  selector: 'app-settings',
  imports: [],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent implements OnInit {
  userId: number = 1; // example - get the actual userId from your login context
  method: string ="";
  secretKey: string="";
  message: string="";

  constructor(private usersService: UsersService) {}

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

  onSaveSettings() {
    this.usersService.update2FASettings(this.userId, this.method, this.secretKey)
      .subscribe({
        next: (res) => {
          this.message = '2FA settings updated.';
        },
        error: (err) => {
          this.message = err.error;
        }
      });
  }
}
