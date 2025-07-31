import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Student } from './student';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private apiUrl = 'https://localhost:7096/Students';

  constructor(private http: HttpClient) {}

  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.apiUrl);
  }

  addStudent(student: Student): Observable<Student> {
  return this.http.post<Student>(this.apiUrl, student);
}

  deleteStudent(id: number): Observable<Student> {
    debugger
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<Student>(url);
  }

  updateStudent(student: Student): Observable<any> {
    debugger
  return this.http.put(this.apiUrl, student);
}

  getStudentById(id: number): Observable<Student> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Student>(url);
  }

  getStudentsPaged(page: number = 1, pageSize: number = 5, sortOrder: string = 'asc',name?: string)
  : Observable<{students: Student[],totalPages: number,totalCount: number,page: number}> {
    let params = `?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
    if (name) {
      params += `&name=${encodeURIComponent(name)}`;
    }
    return this.http.get<{
      students: Student[],
      totalPages: number,
      totalCount: number,
      page: number
    }>(`${this.apiUrl}/paged${params}`);
  }

}
