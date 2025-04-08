import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventEntryContainerComponent } from './event-entry-container.component';

describe('EventEntryContainerComponent', () => {
  let component: EventEntryContainerComponent;
  let fixture: ComponentFixture<EventEntryContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventEntryContainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventEntryContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
