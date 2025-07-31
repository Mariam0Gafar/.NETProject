import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { BehaviorSubject, Subject, debounceTime } from 'rxjs';
import { Course } from '../course';
import { CourseService } from '../course.service';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from "../home.component/home.component";
import { Department } from '../department';
import { DepartmentService } from '../department.service';
@Component({
  selector: 'app-course',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HomeComponent],
  templateUrl: './course.component.html',
  styleUrl: './course.component.css'
})
export class CourseComponent implements OnInit {
  private courseService = inject(CourseService);
  private location = inject(Location);
  constructor(private router: Router, private depService: DepartmentService) {}

  courseList: Course[] = [];
  filteredCourseList$ = new BehaviorSubject<Course[]>([]);

  searchTerm: string = '';
  private searchSubject = new Subject<string>();

  currentPage = 1;
  pageSize = 5;
  totalPages = 0;

  sortOrder: 'asc' | 'desc' = 'asc';

  dep$ = new BehaviorSubject<Department[]>([]);
  depMap$ = new BehaviorSubject<{ [id: number]: string }>({});

  ngOnInit(): void {
    this.loadDeps();
    this.loadCourses();

    this.searchSubject.pipe(debounceTime(300)).subscribe((term) => {
      this.searchTerm = term;
      this.currentPage = 1;
      this.loadCourses();
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
  loadCourses(): void {
    this.courseService
      .getCoursesPaged(this.currentPage, this.pageSize, this.sortOrder, this.searchTerm)
      .subscribe({
        next: (res) => {
          this.courseList = res.courses;
          this.filteredCourseList$.next(this.courseList);
          this.totalPages = res.totalPages;
        },
        error: (err) => console.error('Failed to fetch courses:', err)
      });
  }

  toggleSort(): void {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadCourses();
  }

  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadCourses();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadCourses();
    }
  }

  delete(id: number): void {
    this.courseList = this.courseList.filter(c => c.id !== id);
    this.courseService.deleteCourse(id).subscribe(() => {
      this.loadCourses(); 
    });
  }

  edit(id: number): void {
    this.router.navigate(['/course-update', id]);
  }

  goBack(): void {
    this.location.back();
  }
}
