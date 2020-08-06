import { TestBed, inject } from '@angular/core/testing';

import { FileDownloadService } from './file-download.service';

describe('FileDownloadService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FileDownloadService]
    });
  });

  it('should be created', inject([FileDownloadService], (service: FileDownloadService) => {
    expect(service).toBeTruthy();
  }));
});
