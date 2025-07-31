import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
@Component({
  selector: 'app-logout',
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {

  constructor(private authService: AuthService){}

  logout(){
    this.authService.logout();
  }
}
