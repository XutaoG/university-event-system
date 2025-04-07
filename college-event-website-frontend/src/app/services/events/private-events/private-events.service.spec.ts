import { TestBed } from '@angular/core/testing';

import { PrivateEventsService } from './private-events.service';

describe('PrivateEventsService', () => {
  let service: PrivateEventsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PrivateEventsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
