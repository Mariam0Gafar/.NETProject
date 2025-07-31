import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Exam } from '../exam';
import { ExamService } from '../exam.service';
import { BehaviorSubject, debounceTime, Subject } from 'rxjs';
import { HomeComponent } from '../home.component/home.component';
import { Course } from '../course';
import { CourseService } from '../course.service';
@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HomeComponent],
  templateUrl: './exam.component.html',
  styleUrl: './exam.component.css'
})
export class ExamComponent implements OnInit {
  private examService = inject(ExamService);
  private location = inject(Location);
  constructor(private router: Router, private authService: AuthService, private courseService: CourseService) {
        this.userRoles = this.authService.getCurrentUserRoles();
  }
  searchTerm: string = '';
  private searchSubject = new Subject<string>();
  studentId : number | null = null;
  grades$ = new BehaviorSubject<any[]>([]);
   studentGrade: number | null = null;
    userRoles: string[] = [];

  examList: Exam[] = [];
  filteredExamList$ = new BehaviorSubject<Exam[]>([]);
  courses$ = new BehaviorSubject<Course[]>([]);
  coursesMap$ = new BehaviorSubject<{ [id: number]: string }>({});

    currentPage = 1;
  pageSize = 3;
  totalPages = 0;
  sortOrder: string = 'asc';

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadExams();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadExams();
    }
  }

    toggleSort(): void {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadExams();
  }

  ngOnInit(): void {
    this.loadCourses();
    this.loadExams();
    this.studentId = this.authService.getCurrentUserId(); 
   if(this.studentId!= null){
    this.examService.getGradesForStudent(this.studentId).subscribe({
    next: (gradesList) => {
      this.grades$.next(gradesList);
      console.log("gradess: ", this.grades$.value)
    },
    error: (err) => {
      console.error('Failed to load grades:', err);
    }
  });
}
     this.searchSubject.pipe(debounceTime(300)).subscribe((term) => {
      this.searchTerm = term;
      this.currentPage = 1; 
      this.loadExams();
    });
  }

  goToGrades(id: number): void{
    this.router.navigate(['/grades', id]);
  } 

getGradeForExam(examId: number): number | string {
    if (this.studentId !== null) {
      const sId: number = +this.studentId; 

      const grades = this.grades$.value;
      const match = grades.find(
        g => g.examId === examId && g.studentId === sId 
      );
      return match ? match.grade : 'N/A';
    }
    return 'N/A';
  }
loadCourses(): void{
    this.courseService.getAllCourses().subscribe({
      next: (c) => {
        this.courses$.next(c.courses);
        const map = c.courses.reduce((acc, c) => {
          acc[c.id] = c.name;
          return acc;
        }, {} as { [id: number]: string });
        this.coursesMap$.next(map); 
      },
      error: (err) => console.error('Failed to fetch departments:', err)
    });
  }

  loadExams(): void{
     this.examService.getExamsPaged(this.currentPage, this.pageSize, this.sortOrder,this.searchTerm)
     .subscribe({
      next: (data) => {
        this.examList = data.exams;
        this.filteredExamList$.next(data.exams);
        this.totalPages = data.totalPages;
      },
      error: (err) => console.error('Failed to fetch exams:', err)
    });
  }

  onSortChange(order: string): void {
    this.sortOrder = order;
    this.loadExams(); 
  }

   onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  deleteExam(id: number): void {
    debugger
    this.examList = this.examList.filter(e => e.id !== id);
    this.examService.deleteExam(id).subscribe();
    setTimeout(() => window.location.reload(), 500);
  }

  editExam(id: number): void {
    this.router.navigate(['/exam-update', id]);
  }

  goBack(): void {
    this.location.back();
  }

  isAdmin(): boolean{
    const roles = this.authService.getCurrentUserRoles();
    if(roles.includes('Admin'))
      return true;
    return false;
  }

  hasRole(role: string): boolean {
    return this.userRoles.includes(role);
  }

}
