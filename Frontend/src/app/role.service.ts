import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './User';
import { Observable } from 'rxjs';
import { map } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private apiUrl = 'https://localhost:7096/Roles';
  
  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<User[]>{
    return this.http.get<User[]>(`${this.apiUrl}/users`);
  }

  assignRoles(userId: string, roles: string[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/assign`, { userId, roles });
  }

  getUserRoles(userId: string): Observable<string[]> {
  return this.http.get<string[]>(`${this.apiUrl}/roles?userId=${userId}`)
}

}
