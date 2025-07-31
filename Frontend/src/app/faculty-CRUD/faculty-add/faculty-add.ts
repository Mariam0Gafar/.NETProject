import { Component, inject, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { Faculty } from '../../faculty';
import { FacultyService } from '../../faculty.service';

@Component({
  selector: 'app-faculty-add',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './faculty-add.html',
  styleUrl: './faculty-add.css'
})
export class FacultyAddComponent implements OnInit {
  private location = inject(Location);
  private facultyService = inject(FacultyService);
private router = inject(Router)
  faculties: Faculty[] = [];
  faculties$ = new BehaviorSubject<Faculty[]>([]);

  newFaculty: Faculty = {
    id: 0,
    facultyName: '',
    isActive: true
  };

  ngOnInit(): void {
    this.facultyService.getFaculties().subscribe({
      next: (data) => {
        this.faculties = data;
        this.faculties$.next(data);
      },
      error: (err) => console.error('Failed to fetch faculties:', err)
    });
  }

  addFaculty(): void {
    if (!this.newFaculty.facultyName) return;

    this.facultyService.addFaculty(this.newFaculty).subscribe(() => {
      this.newFaculty = {
        id: 0,
        facultyName: '',
        isActive: true
      };
        this.router.navigate(['/faculties'])
    });
  }

  goBack(): void {
    this.location.back();
  }
}
