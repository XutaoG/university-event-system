import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRsoComponent } from './add-rso.component';

describe('AddRsoComponent', () => {
  let component: AddRsoComponent;
  let fixture: ComponentFixture<AddRsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddRsoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddRsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
