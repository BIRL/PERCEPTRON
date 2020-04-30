import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XicComponent } from './xic.component';

describe('XicComponent', () => {
  let component: XicComponent;
  let fixture: ComponentFixture<XicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
