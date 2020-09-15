import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FdrComponent } from './fdr.component';

describe('FdrComponent', () => {
  let component: FdrComponent;
  let fixture: ComponentFixture<FdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
