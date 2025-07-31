import { Component , OnInit} from '@angular/core';
import { Question } from '../question';
import { Exam } from '../exam';
import { ActivatedRoute } from '@angular/router';
import { ExamService } from '../exam.service';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';
import { AuthService } from '../auth.service';
import { ExamSubmission } from '../ExamSubmission';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-question.component',
  imports: [RouterModule,CommonModule, FormsModule],
  templateUrl: './question.component.html',
  styleUrl: './question.component.css'
})
export class QuestionComponent implements OnInit{
exam$!: Observable<Exam>;

  constructor(
  private route: ActivatedRoute,
  private examService: ExamService,
  private authService: AuthService,
  private cdr: ChangeDetectorRef,
  private router: Router
) {}
  examSubmission!: ExamSubmission;


ngOnInit(): void {
  const id = Number(this.route.snapshot.paramMap.get('id'));
  console.log('Exam ID from route:', id);
  this.exam$ = this.examService.getExamWithQuestions(id);
  const studentId = this.authService.getCurrentUserId();

  if (studentId != null) {
    this.examService.prepareSubmission(id, studentId).subscribe({
      next: (res) => {
        this.examSubmission = res;
        console.log(this.examSubmission);
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('Failed to prepare submission:', err);
      }
    });
  }
}


submit(): void {
  console.log("examsubmitted: ", this.examSubmission)
    this.examService.submitExam(this.examSubmission).subscribe({
      next: (res) => {
        alert(`Exam submitted. Grade: ${res.correct}/${res.total}`);
        this.router.navigate(['/exams']);
      },
      error: (err) => {
        alert(err.error);
      }
    });
  }

}
