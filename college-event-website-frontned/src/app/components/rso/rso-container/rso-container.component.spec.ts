import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RsoContainerComponent } from './rso-container.component';

describe('RsoContainerComponent', () => {
  let component: RsoContainerComponent;
  let fixture: ComponentFixture<RsoContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RsoContainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RsoContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
