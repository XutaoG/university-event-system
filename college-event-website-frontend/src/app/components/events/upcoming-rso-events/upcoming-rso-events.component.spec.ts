import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpcomingRsoEventsComponent } from './upcoming-rso-events.component';

describe('UpcomingRsoEventsComponent', () => {
  let component: UpcomingRsoEventsComponent;
  let fixture: ComponentFixture<UpcomingRsoEventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpcomingRsoEventsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpcomingRsoEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
