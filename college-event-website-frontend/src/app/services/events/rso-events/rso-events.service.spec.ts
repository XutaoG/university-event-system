import { TestBed } from '@angular/core/testing';

import { RsoEventsService } from './rso-events.service';

describe('RsoEventsService', () => {
  let service: RsoEventsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RsoEventsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
