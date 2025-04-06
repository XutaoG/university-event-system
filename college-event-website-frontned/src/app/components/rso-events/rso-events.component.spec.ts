import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RsoEventsComponent } from './rso-events.component';

describe('RsoEventsComponent', () => {
  let component: RsoEventsComponent;
  let fixture: ComponentFixture<RsoEventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RsoEventsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RsoEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
