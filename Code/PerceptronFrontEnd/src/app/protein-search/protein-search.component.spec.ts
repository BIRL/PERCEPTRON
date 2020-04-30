import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProteinSearchComponent } from './protein-search.component';

describe('ProteinSearchComponent', () => {
  let component: ProteinSearchComponent;
  let fixture: ComponentFixture<ProteinSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProteinSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProteinSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
