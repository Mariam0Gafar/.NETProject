import { Component, inject, OnInit } from '@angular/core';
import { Location, CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { Subject, debounceTime } from 'rxjs';
import { Exam } from '../../exam';
import { ExamService } from '../../exam.service';
import { Course } from '../../course';
import { CourseService } from '../../course.service';
import { Question } from '../../question';
import { NgSelectModule } from '@ng-select/ng-select';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-exam-add',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, NgSelectModule],
  templateUrl: './exam-add.html',
  styleUrl: './exam-add.css'
})
export class ExamAddComponent implements OnInit {
  private location = inject(Location);
  private router = inject(Router);
  private examService = inject(ExamService);
  private courseService = inject(CourseService);

  courses: Course[] = [];
  courses$ = new BehaviorSubject<Course[]>([]);

  searchTerm: string = '';
  public searchSubject = new Subject<string>();

  currentPage = 1;
  pageSize = 3;
  totalPages = 0;
  sortOrder: string = 'asc';

  newExam: Exam = {
  id: 0,
  title: '',
  courseId: null,
  isActive: true
};
newQuestions: Question[] = [];

selectedCourse: Course | null = null;
  private searchSub: Subscription | null = null;

  private blankQuestion(): Question {
  return {
    id: 0,
    description: '',
    correctAnswer: '',
    examId: 0
  };
}

addQuestion(): void {
  this.newQuestions.push(this.blankQuestion());
}

removeQuestion(idx: number): void {
  this.newQuestions.splice(idx, 1);
}


ngOnInit(): void {
  this.loadCourses(); 

  this.searchSubject.pipe(
    debounceTime(300)
  ).subscribe(term => {
    this.searchTerm = term;
    this.currentPage = 1;
    this.loadCourses();
  });
}


ngOnDestroy(): void {
  this.searchSub?.unsubscribe();
}

 loadCourses(): void {
  this.courseService.getCoursesPaged(this.currentPage, this.pageSize, this.sortOrder, this.searchTerm)
    .subscribe({
      next: (data) => {
        this.totalPages = data.totalPages;
        this.courses$.next(data.courses); 
      },
      error: (err) => console.error('Failed to fetch courses:', err)
    });
}

  onCourseChange(courseId: number): void {
    this.newExam.courseId = courseId;
  }

 onSearchChange(term: string): void {
  this.searchSubject.next(term);  
}

 addExam(): void {
  if (!this.newExam.title || !this.newExam.courseId) return;

  this.examService.addExam(this.newExam).subscribe({
    next: (createdExam) => {
      const examId = createdExam.id;

      for (let q of this.newQuestions) {
        q.examId = examId;
        this.examService.addQuestion(q).subscribe({
          error: (e) => console.error('Failed to add question:', e)
        });
      }

      this.router.navigate(['/exams']);
    },
    error: (err) => console.error('Failed to add exam:', err)
  });
}

  goBack(): void {
    this.location.back();
  }
}
