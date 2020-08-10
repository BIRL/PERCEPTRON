import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResultsDownloadComponent } from './results-download.component';

describe('ResultsDownloadComponent', () => {
  let component: ResultsDownloadComponent;
  let fixture: ComponentFixture<ResultsDownloadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResultsDownloadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResultsDownloadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
