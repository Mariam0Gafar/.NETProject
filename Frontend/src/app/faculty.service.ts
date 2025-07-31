import { Injectable } from '@angular/core';
import { Faculty } from './faculty';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FacultyService {
  private apiUrl = 'https://localhost:7096/Faculty';

  constructor(private http: HttpClient) { }

  getFaculties(): Observable<Faculty[]>{
    return this.http.get<Faculty[]>(this.apiUrl);
  }

  getFacultyByName(name: string): Observable<Faculty> {
    debugger
    const url = `${this.apiUrl}/name?name=${encodeURIComponent(name)}`;
    return this.http.get<Faculty>(url);
  }

  deleteFaculty(id: number): Observable<Faculty> {
    debugger
      const url = `${this.apiUrl}/${id}`;
      return this.http.delete<Faculty>(url);
    }
  
    updateFaculty(f: Faculty): Observable<any> {
      debugger
    return this.http.put(this.apiUrl, f);
  }
  
    getFacultyById(id: number): Observable<Faculty> {
      const url = `${this.apiUrl}/${id}`;
      return this.http.get<Faculty>(url);
    }
  
      addFaculty(f: Faculty): Observable<Faculty> {
      return this.http.post<Faculty>(this.apiUrl, f);
    }

    getFacultiesPaged(page: number = 1, pageSize: number = 5, sortOrder: string = 'asc',name?: string)
      : Observable<{faculties: Faculty[],totalPages: number,totalCount: number,page: number}> {
        let params = `?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
        if (name) {
          params += `&name=${encodeURIComponent(name)}`;
        }
        return this.http.get<{
          faculties: Faculty[],
          totalPages: number,
          totalCount: number,
          page: number
        }>(`${this.apiUrl}/paged${params}`);
      }
}
