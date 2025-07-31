import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Location } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

import { Faculty } from '../../faculty';
import { FacultyService } from '../../faculty.service';

@Component({
  selector: 'app-faculty-update',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './faculty-update.html',
  styleUrl: './faculty-update.css'
})
export class FacultyUpdateComponent implements OnInit {
  private facultyService = inject(FacultyService);
  private location = inject(Location);

  updatedFaculty: Faculty = {
    id: 0,
    facultyName: '',
    isActive: true
  };

  faculty$ = new BehaviorSubject<Faculty | null>(null);

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (isNaN(id)) {
      console.error('Invalid faculty ID');
      return;
    }

    this.facultyService.getFacultyById(id).subscribe({
      next: (faculty) => {
        this.updatedFaculty = {
          id: id,
          facultyName: faculty.facultyName,
          isActive: faculty.isActive
        };
        this.faculty$.next(this.updatedFaculty);
      },
      error: (err) => console.error('Failed to fetch faculty:', err)
    });
  }

  updateFaculty(): void {
    debugger
    if (!this.updatedFaculty.facultyName) {
      return;
    }

    this.facultyService.updateFaculty(this.updatedFaculty).subscribe({
      next: () => this.router.navigate(['/faculties']),
      error: (err) => console.error('Update failed', err)
    });
  }

  goBack(): void {
    this.location.back();
  }
}
