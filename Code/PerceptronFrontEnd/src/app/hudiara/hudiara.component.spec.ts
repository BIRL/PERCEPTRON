import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HudiaraComponent } from './hudiara.component';

describe('HudiaraComponent', () => {
  let component: HudiaraComponent;
  let fixture: ComponentFixture<HudiaraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HudiaraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HudiaraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
