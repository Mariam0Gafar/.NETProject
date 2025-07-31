import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExamService } from '../../exam.service';
import { Exam } from '../../exam';
import { Question } from '../../question';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BehaviorSubject, Subject , Subscription} from 'rxjs';
import { CourseService } from '../../course.service';
import { Course } from '../../course';
import { NgSelectModule } from '@ng-select/ng-select';
import { debounceTime } from 'rxjs';
@Component({
  selector: 'app-exam-update',
  imports: [CommonModule, RouterModule, FormsModule, NgSelectModule],
  templateUrl: './exam-update.html',
  styleUrls: ['./exam-update.css']
})
export class ExamUpdateComponent implements OnInit {
  examId!: number;
  exam: Exam = {
    id: 0,
    title: '',
    courseId: 0,
    isActive: true
  };
  questions: Question[] = [];
  questions$ = new BehaviorSubject<Question[] | null>(null);
  exam$ = new BehaviorSubject<Exam | null>(null);
  constructor(
    private route: ActivatedRoute,
    private examService: ExamService,
    private router: Router,
    private courseService: CourseService
  ) {}

  courses: Course[] = [];
  courses$ = new BehaviorSubject<Course[]>([]);
  selectedCourseName = '';
searchTerm: string = '';
  public searchSubject = new Subject<string>();
currentPage = 1;
  pageSize = 3;
  totalPages = 0;
  sortOrder: string = 'asc';

  selectedCourse: Course | null = null;
    private searchSub: Subscription | null = null;
  ngOnInit(): void {
  const idParam = this.route.snapshot.paramMap.get('id');
  if (!idParam) {
    alert('No exam ID provided in URL');
    return;
  }

  this.examId = +idParam;

  this.searchSub = this.searchSubject.pipe(
    debounceTime(100)
  ).subscribe(term => {
    this.searchTerm = term;
    this.currentPage = 1;
    this.loadCourses();
  });

  this.loadExamDetails();
  // this.loadCourses(); 
}


loadCourses(): void {
  this.courseService.getCoursesPaged(this.currentPage, this.pageSize, this.sortOrder, this.searchTerm)
    .subscribe({
      next: (data) => {
        this.courses$.next(data.courses);
        this.totalPages = data.totalPages;
        console.log(data.courses);
      },
      error: (err) => alert(`Failed to load courses: ${err.error || err.message}`)
    });
}

loadExamDetails(): void {
  this.examService.getExamById(this.examId).subscribe({
    next: (examData) => {
      this.exam = examData;
      this.exam$.next(examData);
      if(this.exam.courseId != null)
      this.courseService.getCourseById(this.exam.courseId).subscribe({
        next: (course) => {
          this.searchTerm = course.name;
          this.loadCourses(); 
        },
        error: (err) => {
          console.error('Could not fetch course by ID', err);
          this.loadCourses();
        }
      });

      this.examService.getQuestionsByExamId(this.examId).subscribe({
        next: (data) => {
          this.questions = data;
          this.questions$.next(data);
        },
        error: (err) => alert(`Failed to load questions: ${err.error || err.message}`)
      });
    },
    error: (err) => alert(`Failed to load exam: ${err.error || err.message}`)
  });
}

 onSearchChange(term: string): void {
  this.searchSubject.next(term);  
}
  onCourseChange(name: string) {
    this.selectedCourseName = name;
  }

   updateExam(): void {
  this.examService.updateExam(this.exam).subscribe({
    next: () => {
      this.updateAllQuestions();
      alert('Exam and questions updated successfully!');
      this.router.navigate(['/exams']);
    },
    error: (err) => alert(`Failed to update exam: ${err.error || err.message}`)
  });
}


  updateQuestion(question: Question): void {
    this.examService.updateQuestion(question).subscribe({
      error: (err) => alert(`Failed to update question ${question.id}: ${err.error || err.message}`)
    });
  }

  updateAllQuestions(): void {
    this.questions.forEach((question) => this.updateQuestion(question));
  }
}
