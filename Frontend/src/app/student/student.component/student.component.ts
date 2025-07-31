import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentService } from '../student.service';
import { Student } from '../student';
import { BehaviorSubject, debounceTime, Subject } from 'rxjs';
import { Location } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';
import { HomeComponent } from '../../home.component/home.component';
import { Department } from '../../department';
import { DepartmentService } from '../../department.service';
@Component({
  selector: 'app-student',
  imports: [CommonModule, RouterModule, FormsModule, HomeComponent],
  templateUrl: './student.component.html',
  styleUrl: './student.component.css'
})
export class StudentComponent implements OnInit {
  private studentService = inject(StudentService);
  private location = inject(Location);
  constructor(private router: Router, private authService: AuthService, private depService: DepartmentService) {}

  studentList: Student[] = [];
  filteredStudentList$ = new BehaviorSubject<Student[]>([]);

  searchTerm: string = '';
  private searchSubject = new Subject<string>();

  currentPage = 1;
  pageSize = 5;
  totalPages = 0;
  totalCount = 0;

  sortColumn: string = 'name';
  sortOrder: 'asc' | 'desc' = 'asc';

  dep$ = new BehaviorSubject<Department[]>([]);
  depMap$ = new BehaviorSubject<{ [id: number]: string }>({});

  ngOnInit(): void {
    this.loadDeps();
    this.loadStudents();

    this.searchSubject.pipe(debounceTime(300)).subscribe((term) => {
      this.searchTerm = term;
      this.currentPage = 1; 
      this.loadStudents();
    });
  }

  loadDeps(): void{
    this.depService.getDepartments().subscribe({
      next: (deps) => {
        this.dep$.next(deps);
        const map = deps.reduce((acc, f) => {
          acc[f.id] = f.name;
          return acc;
        }, {} as { [id: number]: string });
        this.depMap$.next(map); 
        console.log("fac: ", this.depMap$.value)
      },
      error: (err) => console.error('Failed to fetch departments:', err)
    });
  }
  loadStudents(): void {
    this.studentService.getStudentsPaged(
      this.currentPage,
      this.pageSize,
      this.sortOrder,
      this.searchTerm
    ).subscribe({
      next: (data) => {
        this.studentList = data.students;
        this.filteredStudentList$.next(data.students);
        this.totalPages = data.totalPages;
        this.totalCount = data.totalCount;
      },
      error: (err) => console.error('Failed to fetch students:', err)
    });
  }

  toggleSort(): void {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadStudents();
  }

  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadStudents();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadStudents();
    }
  }

  delete(id: number): void {
    this.studentList = this.studentList.filter(s => s.id !== id);
    this.studentService.deleteStudent(id).subscribe(() => {
      this.loadStudents();
    });
  }

  editStudent(id: number): void {
    this.router.navigate(['/student-update', id]);
  }

  isCurrentUser(email: string): boolean {
    const currentEmail = this.authService.getCurrentUserEmail();
    console.log("current: ",currentEmail);
    console.log(email);
    return currentEmail === email;
  }

  getCurrentUserRoles(): string[] {
    return this.authService.getCurrentUserRoles();
  }

  isAdmin(): boolean{
    const roles = this.getCurrentUserRoles();
    if(roles.includes('Admin'))
      return true;
    return false;
  }


}
