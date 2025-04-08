import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventModalViewComponent } from './event-modal-view.component';

describe('EventModalViewComponent', () => {
  let component: EventModalViewComponent;
  let fixture: ComponentFixture<EventModalViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventModalViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventModalViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
