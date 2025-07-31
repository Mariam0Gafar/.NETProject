import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:7096/Account'; 
  private tokenKey = 'jwt_token';

  isLoggedIn$ = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient, private router: Router) {}

  register(user: any) {
    return this.http.post(`${this.baseUrl}/Register`, user);
  }

  login(credentials: any) {
    return this.http.post<any>(`${this.baseUrl}/Login`, credentials).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
        this.isLoggedIn$.next(true);
      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getCurrentUserEmail(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
    } catch (err) {
      return null;
    }
  }

  getCurrentUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      return decoded["studentId"];
    } catch (err) {
      return null;
    }
  }

 getCurrentUserRoles(): string[] {
  const token = this.getToken();
  if (!token) return [];
  try {
    const decoded: any = jwtDecode(token);
    const roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return Array.isArray(roles) ? roles : [roles]; 
  } catch (err) {
    return [];
  }
}


private hasToken(): boolean {
  const token = localStorage.getItem(this.tokenKey);
  return !!token && !this.isTokenExpired();
}


  isTokenExpired(): boolean {
  const token = this.getToken();
  if (!token) return true;

  try {
    const decoded: any = jwtDecode(token);
    const exp = decoded.exp; 
    const now = Math.floor(Date.now() / 1000);
    return now > exp;
  } catch (err) {
    return true; 
  }
}

logout() {
  localStorage.removeItem(this.tokenKey);
  localStorage.removeItem('token');
  this.isLoggedIn$.next(false);
  this.router.navigate(['/login']);
}
}
