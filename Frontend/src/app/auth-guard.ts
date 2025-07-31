import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { ActivatedRouteSnapshot } from '@angular/router';
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const token = localStorage.getItem('token');
    if (!token || this.authService.isTokenExpired()) {
      this.router.navigate(['/login']);
      return false;
    }

    const allowedRoles = route.data['roles'] as string[] | undefined;
    const userRoles = this.authService.getCurrentUserRoles(); 

    if (!allowedRoles || userRoles.some(role => allowedRoles.includes(role))) {
      return true;
    }

    this.router.navigate(['/unauthorized']);
    return false;
      }
  
}
