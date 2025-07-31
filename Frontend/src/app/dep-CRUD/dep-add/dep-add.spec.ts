import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepAdd } from './dep-add';

describe('DepAdd', () => {
  let component: DepAdd;
  let fixture: ComponentFixture<DepAdd>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepAdd]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepAdd);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
