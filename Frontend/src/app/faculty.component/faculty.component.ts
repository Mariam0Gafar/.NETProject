import { Component, inject, OnInit } from '@angular/core';
import { FacultyService } from '../faculty.service';
import { Faculty } from '../faculty';
import { BehaviorSubject, Subject, debounceTime } from 'rxjs';
import { CommonModule, Location } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from '../home.component/home.component';
@Component({
  selector: 'app-faculty',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HomeComponent],
  templateUrl: './faculty.component.html',
  styleUrl: './faculty.component.css'
})
export class FacultyComponent implements OnInit {
  private facultyService = inject(FacultyService);
  private location = inject(Location);
  constructor(private router: Router) {}

  facultyList: Faculty[] = [];
  filteredFacultyList$ = new BehaviorSubject<Faculty[]>([]);

  searchTerm: string = '';
  private searchSubject = new Subject<string>();

  currentPage = 1;
  pageSize = 5;
  totalPages = 0;

  sortOrder: 'asc' | 'desc' = 'asc';

  ngOnInit(): void {
    this.loadFaculties();

    this.searchSubject.pipe(debounceTime(300)).subscribe((term) => {
      this.searchTerm = term;
      this.currentPage = 1;
      this.loadFaculties();
    });
  }

  loadFaculties(): void {
    this.facultyService
      .getFacultiesPaged(this.currentPage, this.pageSize, this.sortOrder, this.searchTerm)
      .subscribe({
        next: (data) => {
          this.facultyList = data.faculties;
          this.filteredFacultyList$.next(data.faculties);
          this.totalPages = data.totalPages;
        },
        error: (err) => console.error('Failed to fetch faculties:', err)
      });
  }

  toggleSort(): void {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadFaculties();
  }

  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadFaculties();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadFaculties();
    }
  }

  delete(id: number): void {
    this.facultyList = this.facultyList.filter(f => f.id !== id);
    this.facultyService.deleteFaculty(id).subscribe(() => {
      this.loadFaculties();
    });
  }

  edit(id: number): void {
    this.router.navigate(['/faculty-update', id]);
  }

  goBack(): void {
    this.location.back();
  }
}
