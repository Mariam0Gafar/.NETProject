import { Component, inject, OnInit } from '@angular/core';
import { Department } from '../department';
import { DepartmentService } from '../department.service';
import { BehaviorSubject, Subject, debounceTime } from 'rxjs';
import { CommonModule, Location } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from '../home.component/home.component';
import { Faculty } from '../faculty';
import { FacultyService } from '../faculty.service';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-department',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HomeComponent],
  templateUrl: './department.component.html',
  styleUrl: './department.component.css'
})
export class DepartmentComponent implements OnInit {
  private depService = inject(DepartmentService);
  private location = inject(Location);
  constructor(private router: Router, private facultyService: FacultyService, private cdr: ChangeDetectorRef) {}

  departmentList: Department[] = [];
  filteredDepList$ = new BehaviorSubject<Department[]>([]);

  searchTerm: string = '';
  private searchSubject = new Subject<string>();

  currentPage = 1;
  pageSize = 5;
  totalPages = 0;

  sortOrder: 'asc' | 'desc' = 'asc';

  faculties$ = new BehaviorSubject<Faculty[]>([]);
  facultyMap$ = new BehaviorSubject<{ [id: number]: string }>({});

  ngOnInit(): void {
        this.loadFaculties();
    this.loadDepartments();
    this.searchSubject.pipe(debounceTime(300)).subscribe((term) => {
      this.searchTerm = term;
      this.currentPage = 1;
      this.loadDepartments();
    });
  }

  loadFaculties(): void {
    this.facultyService.getFaculties().subscribe({
      next: (faculties) => {
        this.faculties$.next(faculties);
        const map = faculties.reduce((acc, f) => {
          acc[f.id] = f.facultyName;
          return acc;
        }, {} as { [id: number]: string });
        this.facultyMap$.next(map); 
        console.log("fac: ", this.facultyMap$.value)
      },
      error: (err) => console.error('Failed to fetch faculties:', err)
    });
}

  loadDepartments(): void {
    this.depService
      .getDepsPaged(this.currentPage, this.pageSize, this.sortOrder, this.searchTerm)
      .subscribe({
        next: (data) => {
          this.departmentList = data.departments;
          this.filteredDepList$.next(data.departments);
          this.totalPages = data.totalPages;
         
        },
        error: (err) => console.error('Failed to fetch departments:', err)
      });
  }

  
  toggleSort(): void {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadDepartments();
  }

  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadDepartments();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadDepartments();
    }
  }

  delete(id: number): void {
    this.departmentList = this.departmentList.filter(d => d.id !== id);
    this.depService.deleteDep(id).subscribe(() => {
      this.loadDepartments();
    });
  }

  edit(id: number): void {
    this.router.navigate(['/dep-update', id]);
  }

  goBack(): void {
    this.location.back();
  }
}
