import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerticalContainerComponent } from './vertical-container.component';

describe('VerticalContainerComponent', () => {
  let component: VerticalContainerComponent;
  let fixture: ComponentFixture<VerticalContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerticalContainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerticalContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
