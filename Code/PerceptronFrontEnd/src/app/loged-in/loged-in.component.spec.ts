import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogedInComponent } from './loged-in.component';

describe('LogedInComponent', () => {
  let component: LogedInComponent;
  let fixture: ComponentFixture<LogedInComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogedInComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogedInComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
