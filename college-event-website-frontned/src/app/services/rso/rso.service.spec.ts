import { TestBed } from '@angular/core/testing';

import { RsoService } from './rso.service';

describe('RsoService', () => {
  let service: RsoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RsoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
