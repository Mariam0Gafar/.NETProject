import { Component, inject, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

import { Department } from '../../department';
import { DepartmentService } from '../../department.service';
import { CourseService } from '../../course.service';
import { Course } from '../../course'; 
import { Router } from '@angular/router';
@Component({
  selector: 'app-course-add',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './course-add.html',
  styleUrls: ['./course-add.css']
})
export class CourseAddComponent implements OnInit {
  private location = inject(Location);
  private departmentService = inject(DepartmentService);
  private courseService = inject(CourseService);
  private router = inject(Router);

  departments: Department[] = [];
  departments$ = new BehaviorSubject<Department[]>([]);

  newCourse: Course = {
    id: 0,
    name: '',
    description: '',
    departmentId: undefined
  };

  selectedDepartmentName = '';

  ngOnInit(): void {
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.departments$.next(data);
      },
      error: (err) => console.error('Failed to fetch departments:', err)
    });
  }

  onDepartmentChange(name: string) {
    this.selectedDepartmentName = name;
  }

  addCourse(): void {
  if (!this.newCourse.name || !this.newCourse.description) {
    return;
  }

  if (this.selectedDepartmentName) {
    this.departmentService.getDepByName(this.selectedDepartmentName).subscribe(dep => {
      this.newCourse.departmentId = dep.id;
      this.saveCourse();
    });
  } else {
    this.newCourse.departmentId = undefined;
    this.saveCourse();
  }
}

private saveCourse(): void {
  this.courseService.addCourse(this.newCourse).subscribe(() => {
    this.newCourse = {
      id: 0,
      name: '',
      description: '',
      departmentId: undefined
    };
    this.selectedDepartmentName = '';
    setTimeout(() => window.location.reload(), 300);
    this.router.navigate(['/courses']);
  });
}


  goBack() {
    this.location.back();
  }
}
