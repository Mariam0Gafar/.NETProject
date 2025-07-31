import { Component, inject, OnInit } from '@angular/core';
import { Department } from '../../department';
import { DepartmentService } from '../../department.service';
import { Faculty } from '../../faculty';
import { FacultyService } from '../../faculty.service';
import { Location, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-dep-update',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './dep-update.html',
  styleUrl: './dep-update.css'
})
export class DepUpdateComponent implements OnInit {
  private departmentService = inject(DepartmentService);
  private location = inject(Location);

  updatedDepartment: Department = {
    id: 0,
    name: '',
    description: '',
    facultyId: 0
  };

  department$ = new BehaviorSubject<Department | null>(null);

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (isNaN(id)) {
      console.error('Invalid department ID');
      return;
    }

    this.departmentService.getDepById(id).subscribe({
      next: (dep) => {
        this.updatedDepartment = { ...dep };
        this.department$.next(this.updatedDepartment);
      },
      error: (err) => console.error('Failed to fetch department:', err)
    });
  }

  updateDepartment(): void {
    if (!this.updatedDepartment.name || !this.updatedDepartment.description) {
      return;
    }

    this.departmentService.updateDep(this.updatedDepartment).subscribe({
      next: () => {
        this.router.navigate(['/departments']);
      },
      error: (err) => {
        console.error('Update failed', err);
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
