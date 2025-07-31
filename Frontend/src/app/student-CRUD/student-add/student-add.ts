import { Component , inject, OnInit} from '@angular/core';
import { Student } from '../../student/student';
import { StudentService } from '../../student/student.service';
import { Location } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Department } from '../../department';
import { DepartmentService } from '../../department.service';
import { CommonModule } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
@Component({
  selector: 'app-student-add',
  imports: [CommonModule,FormsModule, RouterModule],
  templateUrl: './student-add.html',
  styleUrl: './student-add.css'
})
export class StudentAddComponent implements OnInit{
  private studentService = inject(StudentService);
  private location = inject(Location);
  private departmentService = inject(DepartmentService);
private router = inject(Router);
  departments: Department[] = [];
  departments$ = new BehaviorSubject<Department[]>([]);

  newStudent: Student = {
    id: 0,
    name: '',
    email: '',
    phoneNumber: '',
    departmentId: 1
  };

  selectedDepartmentName = '';
 
ngOnInit(): void {
  this.departmentService.getDepartments().subscribe({
    next: (data) =>{
      this.departments = data;
      this.departments$.next(data); 
      if (data.length > 0) {
      this.selectedDepartmentName = data[0].name;
      this.onDepartmentChange(this.selectedDepartmentName); 
    }
    },
    error: (err) => console.error('Failed to fetch departments:', err)
  });
  }
  
  onDepartmentChange(name: string) {
  this.selectedDepartmentName = name;
}

  addStudent(): void {
  if (!this.newStudent.name || !this.newStudent.email || !this.newStudent.phoneNumber) {
    return;
  }

  this.departmentService.getDepByName(this.selectedDepartmentName).subscribe(dep => {
    this.newStudent.departmentId = dep.id;

    this.studentService.addStudent(this.newStudent).subscribe(() => {
      this.newStudent = {
        id: 0,
        name: '',
        email: '',
        phoneNumber: '',
        departmentId: 0
      };
        this.router.navigate(['/students'])
    });
  });
}
    goBack(){
      this.location.back();
  }
}
