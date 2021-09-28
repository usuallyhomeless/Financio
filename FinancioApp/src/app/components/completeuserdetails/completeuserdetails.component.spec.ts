import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompleteuserdetailsComponent } from './completeuserdetails.component';

describe('CompleteuserdetailsComponent', () => {
  let component: CompleteuserdetailsComponent;
  let fixture: ComponentFixture<CompleteuserdetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompleteuserdetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompleteuserdetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
