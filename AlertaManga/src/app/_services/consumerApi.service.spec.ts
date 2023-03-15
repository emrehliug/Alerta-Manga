/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ConsumerApiService } from './consumerApi.service';

describe('Service: ConsumerApi', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ConsumerApiService]
    });
  });

  it('should ...', inject([ConsumerApiService], (service: ConsumerApiService) => {
    expect(service).toBeTruthy();
  }));
});
