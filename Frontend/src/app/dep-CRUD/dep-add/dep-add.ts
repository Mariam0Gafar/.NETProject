import { Component , inject, OnInit} from '@angular/core';
import { Location } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Department } from '../../department';
import { DepartmentService } from '../../department.service';
import { CommonModule } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Faculty } from '../../faculty';
import { FacultyService } from '../../faculty.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-dep-add',
  imports: [CommonModule, FormsModule,RouterModule],
  templateUrl: './dep-add.html',
  styleUrl: './dep-add.css'
})
export class DepAddComponent implements OnInit{
  private location = inject(Location);
  private departmentService = inject(DepartmentService);
private facultyService = inject(FacultyService);
private router = inject(Router);
  faculties: Faculty[] = [];
  faculties$ = new BehaviorSubject<Faculty[]>([]);

  departments: Department[] = [];
  departments$ = new BehaviorSubject<Department[]>([]);

  newDep: Department = {
    id: 0,
    name: '',
    description: '',
    facultyId: 0
  };
  selectedFacultyName = '';

  ngOnInit(): void {
  this.facultyService.getFaculties().subscribe({
    next: (data) =>{
      this.faculties = data;
      this.faculties$.next(data); 
      if (data.length > 0) {
      this.selectedFacultyName = data[0].facultyName;
      this.onFacultyChange(this.selectedFacultyName); 
    }
    },
    error: (err) => console.error('Failed to fetch faculties:', err)
  });
  }
  
  onFacultyChange(name: string) {
  this.selectedFacultyName = name;
}
  addDep(): void {
    debugger
  if (!this.newDep.name || !this.newDep.description) {
    return;
  }

    this.facultyService.getFacultyByName(this.selectedFacultyName).subscribe(f => {
      this.newDep.facultyId = f.id;
      this.departmentService.addDep(this.newDep).subscribe({
        next:() => {
        this.newDep = {
          id: 0,
          name: '',
          description: '',
          facultyId: 0
        };
        this.router.navigate(['/departments'])
      },
      error: (err) => {
        alert(err.error);
      }
      });
    });

}
    goBack(){
      this.location.back();
  }
}
