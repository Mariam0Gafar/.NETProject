import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExamService } from '../exam.service';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { RouterModule } from '@angular/router';
import { Student } from '../student/student';
import { StudentService } from '../student/student.service';
@Component({
  selector: 'app-grade.component',
  imports: [CommonModule, RouterModule],
  templateUrl: './grade.component.html',
  styleUrl: './grade.component.css'
})
export class GradeComponent implements OnInit {
  grades$ = new BehaviorSubject<any[]>([]);
  examId!: number;
  sortOrder : 'asc' | 'desc' = 'asc'
  students$ = new BehaviorSubject<Student[]>([]);
    sMap$ = new BehaviorSubject<{ [id: number]: string }>({});
  constructor(private route: ActivatedRoute, private examService: ExamService, private studentService: StudentService){}
ngOnInit(): void {
 this.loadStudents();
  this.examId = Number(this.route.snapshot.paramMap.get('id'));  
  this.examService.getStudentExams(this.sortOrder).subscribe(data => {
    if (data) {
      this.grades$.next(data.filter(g => g.examId === this.examId));
      console.log("grades: ", this.grades$);
    }
  });
}

loadStudents(): void{
    this.studentService.getStudents().subscribe({
      next: (s) => {
        this.students$.next(s);
        const map = s.reduce((acc, f) => {
          acc[f.id] = f.name;
          return acc;
        }, {} as { [id: number]: string });
        this.sMap$.next(map); 
        console.log("students: ", this.sMap$.value)
      },
      error: (err) => console.error('Failed to fetch students:', err)
    });
  }

}
