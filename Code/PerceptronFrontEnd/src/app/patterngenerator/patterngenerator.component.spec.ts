import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PatterngeneratorComponent } from './patterngenerator.component';

describe('PatterngeneratorComponent', () => {
  let component: PatterngeneratorComponent;
  let fixture: ComponentFixture<PatterngeneratorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PatterngeneratorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PatterngeneratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
