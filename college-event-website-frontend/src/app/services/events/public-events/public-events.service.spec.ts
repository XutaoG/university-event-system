import { TestBed } from '@angular/core/testing';

import { PublicEventsService } from './public-events.service';

describe('PublicEventsService', () => {
  let service: PublicEventsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PublicEventsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
