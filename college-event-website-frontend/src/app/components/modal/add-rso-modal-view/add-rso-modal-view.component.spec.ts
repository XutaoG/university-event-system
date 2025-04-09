import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRsoModalViewComponent } from './add-rso-modal-view.component';

describe('AddRsoModalViewComponent', () => {
  let component: AddRsoModalViewComponent;
  let fixture: ComponentFixture<AddRsoModalViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddRsoModalViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddRsoModalViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
