import { Component ,inject, OnInit} from '@angular/core';
import { Student } from '../../student/student';
import { StudentService } from '../../student/student.service';
import { Location } from '@angular/common';
import { Department } from '../../department';
import { BehaviorSubject } from 'rxjs';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { DepartmentService } from '../../department.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-student-update',
  imports: [CommonModule,RouterModule, FormsModule],
  templateUrl: './student-update.html',
  styleUrl: './student-update.css'
})
export class StudentUpdateComponent implements OnInit{
  private studentService = inject(StudentService);
  private location = inject(Location);
  private departmentService = inject(DepartmentService);

  updatedStudent: Student = {
  id: 0,
  name: '',
  email: '',
  phoneNumber: '',
  departmentId: 1 
};


 student$ = new BehaviorSubject<Student | null>(null);

constructor(private route: ActivatedRoute, private router: Router) {}

ngOnInit(): void {
  debugger
  const id = Number(this.route.snapshot.paramMap.get('id'));
  if (isNaN(id)) {
    console.error('Invalid student ID');
    return;
  }

  this.studentService.getStudentById(id).subscribe({
    next: (student) => {
      this.updatedStudent.name = student.name;
      this.updatedStudent.email = student.email;
      this.updatedStudent.phoneNumber = student.phoneNumber;
      this.updatedStudent.id = id;
      this.student$.next(this.updatedStudent);
    },
    error: (err) => console.error('Failed to fetch student:', err)
  });
}

updateStudent(): void {
  if (!this.updatedStudent.id || !this.updatedStudent.name || !this.updatedStudent.email || !this.updatedStudent.phoneNumber) {
    return;
  }

  this.studentService.updateStudent(this.updatedStudent).subscribe({
    next: () => {
      this.router.navigate(['/students']);
    },
    error: (err) => {
      console.error('Update failed', err);
    }
  });
}

}
