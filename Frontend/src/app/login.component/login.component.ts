import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-login.component',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
loginData = {
    email: '',
    password: ''
  };
  error: string | null = null;

  msg: string | null = null;

  constructor(private auth: AuthService, private router: Router, private cdr: ChangeDetectorRef) {}

  onLogin() {
  this.auth.login(this.loginData).subscribe({
    next: (response: any) => {
      console.log('Login response:', response); 
      localStorage.setItem('token', response.token);
      console.log('Saved token:', localStorage.getItem('token')); 

      this.msg = 'Successfully logged in!';
      this.error = null;
      this.cdr.detectChanges();

      setTimeout(() => {
        this.router.navigate(['/home']);
      }, 1000);
    },
    error: () => {
      this.error = 'Invalid credentials';
      this.msg = null;
      this.cdr.detectChanges();
    }
  });
}



  register(){
    this.router.navigate(['/register']);
  }
}
