import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-passwordBox',
  standalone: true,
  imports: [FormsModule, NgClass],
  templateUrl: './passwordBox.component.html',
  styleUrl: './passwordBox.component.css',
})
export class PasswordBoxComponent {
  @Input() passwordValue = ''; // Input property for binding the password value
  @Input() placeholder = 'Enter password'; // Default placeholder value
  @Output() passwordValueChange = new EventEmitter<string>(); // Output property to emit changes
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

  onPasswordChange(value: string): void {
    this.passwordValue = value;
    this.passwordValueChange.emit(this.passwordValue); // Emit the updated value to the parent
  }
}
