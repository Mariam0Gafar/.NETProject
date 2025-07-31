import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepUpdate } from './dep-update';

describe('DepUpdate', () => {
  let component: DepUpdate;
  let fixture: ComponentFixture<DepUpdate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepUpdate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepUpdate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
