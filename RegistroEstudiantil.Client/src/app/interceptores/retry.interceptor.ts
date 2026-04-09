// ✅ 4. RetryInterceptor (Reintenta si hay fallo temporal)
import { HttpInterceptorFn } from '@angular/common/http';
import { retry } from 'rxjs';

export const retryInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    retry(2) // Reintenta hasta 2 veces si falla
  );
};
