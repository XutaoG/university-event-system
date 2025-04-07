import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventNavComponent } from './event-nav.component';

describe('EventNavComponent', () => {
  let component: EventNavComponent;
  let fixture: ComponentFixture<EventNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventNavComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
