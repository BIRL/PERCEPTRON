import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanViewComponent } from './scan-view.component';

describe('ScanViewComponent', () => {
  let component: ScanViewComponent;
  let fixture: ComponentFixture<ScanViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScanViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScanViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
