import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/LoadingService';


export const loadingInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const loadingService = inject(LoadingService);

  loadingService.mostrar();

  return next(req).pipe(
    finalize(() => loadingService.ocultar())
  );
};
