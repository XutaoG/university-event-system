import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEventModalViewComponent } from './add-event-modal-view.component';

describe('AddEventModalViewComponent', () => {
  let component: AddEventModalViewComponent;
  let fixture: ComponentFixture<AddEventModalViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddEventModalViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEventModalViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
