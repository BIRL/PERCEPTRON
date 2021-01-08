import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResultsinterpretationComponent } from './resultsinterpretation.component';

describe('ResultsinterpretationComponent', () => {
  let component: ResultsinterpretationComponent;
  let fixture: ComponentFixture<ResultsinterpretationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResultsinterpretationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResultsinterpretationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
