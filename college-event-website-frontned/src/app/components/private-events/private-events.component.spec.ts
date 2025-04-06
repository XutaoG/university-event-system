import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivateEventsComponent } from './private-events.component';

describe('PrivateEventsComponent', () => {
  let component: PrivateEventsComponent;
  let fixture: ComponentFixture<PrivateEventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrivateEventsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrivateEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
