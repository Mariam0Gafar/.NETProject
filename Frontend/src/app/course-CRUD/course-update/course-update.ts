import { Component, inject, OnInit } from '@angular/core';
import { Course } from '../../course';
import { CourseService } from '../../course.service';
import { Department } from '../../department';
import { DepartmentService } from '../../department.service';
import { Location, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-course-update',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './course-update.html',
  styleUrl: './course-update.css'
})
export class CourseUpdateComponent implements OnInit {
  private courseService = inject(CourseService);
  private departmentService = inject(DepartmentService);
  private location = inject(Location);


  updatedCourse: Course = {
    id: 0,
    name: '',
    description: '',
    departmentId: undefined
  };

  course$ = new BehaviorSubject<Course | null>(null);

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (isNaN(id)) {
      console.error('Invalid course ID');
      return;
    }
    this.courseService.getCourseById(id).subscribe({
      next: (course) => {
        this.updatedCourse = { ...course }; 
        this.course$.next(this.updatedCourse);
      },
      error: (err) => console.error('Failed to fetch course:', err)
    });
  }



  updateCourse(): void {
    if (!this.updatedCourse.name || !this.updatedCourse.description) {
      return;
    }

    this.courseService.updateCourse(this.updatedCourse).subscribe({
      next: () => this.router.navigate(['/courses']),
      error: (err) => console.error('Update failed', err)
    });
  }

  goBack(): void {
    this.location.back();
  }
}
