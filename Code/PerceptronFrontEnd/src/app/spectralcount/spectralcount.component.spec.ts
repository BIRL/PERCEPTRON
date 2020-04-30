import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpectralcountComponent } from './spectralcount.component';

describe('SpectralcountComponent', () => {
  let component: SpectralcountComponent;
  let fixture: ComponentFixture<SpectralcountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpectralcountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpectralcountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
