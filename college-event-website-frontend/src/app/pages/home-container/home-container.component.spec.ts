import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeContainerComponent } from './home-container.component';

describe('HomeContainerComponent', () => {
  let component: HomeContainerComponent;
  let fixture: ComponentFixture<HomeContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeContainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
