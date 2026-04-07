import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { finalize } from 'rxjs';

export const timingInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const inicio = Date.now(); // Marca de inicio

  return next(req).pipe(
    finalize(() => {
      const fin = Date.now();
      const duracion = fin - inicio;
      console.log(`⏱️ [${req.method}] ${req.url} tomó ${duracion} ms`);
    })
  );
};
