import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { redirectHomeGuard } from './redirect-home.guard';

describe('redirectHomeGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => redirectHomeGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
