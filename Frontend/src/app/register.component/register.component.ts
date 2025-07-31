import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { DepartmentService } from '../department.service';
import { Department } from '../department';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-register.component',
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerData = {
    email: '',
    password: '',
    name: '',
    phoneNumber: '',
    departmentId: 0
  };

  departments: Department[] = [];
  error: string | null = null;
  msg: string | null = null;
  constructor(
    private auth: AuthService,
    private departmentService: DepartmentService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.departmentService.getDepartments().subscribe({
      next: (departments) => this.departments = departments,
      error: () => this.error = 'Failed to load departments'
    });
  }

  onRegister() {
    if (this.registerData.departmentId === 0) {
      this.error = 'Please select a department';
      return;
    }

    this.auth.register(this.registerData).subscribe({
      next: () => {
      this.msg = 'Registeration Successful!';
      this.error = null;
      this.cdr.detectChanges();

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1000);
      },
      error: () => {
        this.error = 'Invalid Inputs';
        this.cdr.detectChanges();
      }
    });
  }
}
