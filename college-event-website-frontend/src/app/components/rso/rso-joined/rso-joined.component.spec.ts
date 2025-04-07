import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RsoJoinedComponent } from './rso-joined.component';

describe('RsoJoinedComponent', () => {
  let component: RsoJoinedComponent;
  let fixture: ComponentFixture<RsoJoinedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RsoJoinedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RsoJoinedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
