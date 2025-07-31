import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseUpdate } from './course-update';

describe('CourseUpdate', () => {
  let component: CourseUpdate;
  let fixture: ComponentFixture<CourseUpdate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseUpdate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseUpdate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
