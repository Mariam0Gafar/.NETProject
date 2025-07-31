import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Department } from './department';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private apiUrl = 'https://localhost:7096/Departments';

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<Department[]>{
    return this.http.get<Department[]>(this.apiUrl);
  }

  getDepByName(name: string): Observable<Department> {
    debugger
  const url = `${this.apiUrl}/name?name=${encodeURIComponent(name)}`;
  return this.http.get<Department>(url);
}

deleteDep(id: number): Observable<Department> {
  debugger
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<Department>(url);
  }

  updateDep(dep: Department): Observable<any> {
    debugger
  return this.http.put(this.apiUrl, dep);
}

  getDepById(id: number): Observable<Department> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Department>(url);
  }

    addDep(dep: Department): Observable<Department> {
    return this.http.post<Department>(this.apiUrl, dep);
  }

  getDepsPaged(page: number = 1, pageSize: number = 5, sortOrder: string = 'asc',name?: string)
        : Observable<{departments: Department[],totalPages: number,totalCount: number,page: number}> {
          let params = `?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
          if (name) {
            params += `&name=${encodeURIComponent(name)}`;
          }
          return this.http.get<{
            departments: Department[],
            totalPages: number,
            totalCount: number,
            page: number
          }>(`${this.apiUrl}/paged${params}`);
        }
}
