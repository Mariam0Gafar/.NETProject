import { Component } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { LogoutComponent } from '../logout.component/logout.component';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-home',
  imports: [RouterOutlet, RouterModule, LogoutComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
    userRoles: string[] = [];
  constructor(private authService: AuthService){
    this.userRoles = this.authService.getCurrentUserRoles();
  }
hasRole(role: string): boolean {
    return this.userRoles.includes(role) || this.userRoles.includes('Admin');
  }

}
