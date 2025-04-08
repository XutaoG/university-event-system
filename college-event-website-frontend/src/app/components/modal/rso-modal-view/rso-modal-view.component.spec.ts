import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RsoModalViewComponent } from './rso-modal-view.component';

describe('RsoModalViewComponent', () => {
  let component: RsoModalViewComponent;
  let fixture: ComponentFixture<RsoModalViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RsoModalViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RsoModalViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
