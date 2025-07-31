import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FacultyUpdate } from './faculty-update';

describe('FacultyUpdate', () => {
  let component: FacultyUpdate;
  let fixture: ComponentFixture<FacultyUpdate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FacultyUpdate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FacultyUpdate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
