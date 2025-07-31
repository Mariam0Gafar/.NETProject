import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from './course';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private apiUrl = 'https://localhost:7096/Courses';

  constructor(private http: HttpClient) { }

getCourses(page: number = 1, pageSize: number = 5, sortOrder: string = 'asc'): Observable<{courses: Course[],totalPages: number,totalCount: number,page: number}> {
  return this.http.get<{courses: Course[],totalPages: number,totalCount: number,page: number}>
  (`${this.apiUrl}?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`);
}

getCoursesPaged(page: number = 1, pageSize: number = 5, sortOrder: string = 'asc',name?: string): Observable<{courses: Course[],totalPages: number,totalCount: number,page: number}> {
  let params = `?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
  if (name) {
    params += `&name=${encodeURIComponent(name)}`;
  }
  return this.http.get<{
    courses: Course[],
    totalPages: number,
    totalCount: number,
    page: number
  }>(`${this.apiUrl}/paged${params}`);
}

getAllCourses(): Observable<{courses: Course[]}>{
    return this.http.get<{courses: Course[]}>(this.apiUrl);
  }
  deleteCourse(id: number): Observable<Course> {
    debugger
      const url = `${this.apiUrl}/${id}`;
      return this.http.delete<Course>(url);
    }
  
    updateCourse(c: Course): Observable<any> {
      debugger
    return this.http.put(this.apiUrl, c);
  }
  
    getCourseById(id: number): Observable<Course> {
      const url = `${this.apiUrl}/${id}`;
      return this.http.get<Course>(url);
    }
  
      addCourse(c: Course): Observable<Course> {
      return this.http.post<Course>(this.apiUrl, c);
    }
    getCourseByName(name: string): Observable<Course> {
        debugger
        const url = `${this.apiUrl}/name?name=${encodeURIComponent(name)}`;
        return this.http.get<Course>(url);
      }
}
