import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { ErrorService } from '../services/ErrorService';
export const errorInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const router = inject(Router);
  const seguridadService = inject(AuthService);
  const errorService = inject(ErrorService); // 👈 inyecta el servicio

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 0:
          errorService.mostrar('⚠️ No se pudo conectar al servidor.');
          break;

        case 400:
          errorService.mostrar('❌ Datos inválidos. Verifica lo que enviaste.');
          console.warn('Detalles (400):', error.error);
          break;

        case 401:
          errorService.mostrar('🔐 Tu sesión ha expirado.');
          seguridadService.logout();
          router.navigate(['/login']);
          break;

        case 403:
          errorService.mostrar('🚫 No tienes permisos para esta acción.');
          break;

        case 404:
          errorService.mostrar('🔍 No se encontró el recurso solicitado.');
          break;

        case 500:
          errorService.mostrar('💥 Error del servidor. Intenta más tarde.');
          break;

        default:
          errorService.mostrar('❗ Ocurrió un error inesperado.');
          console.error('Error no controlado:', error);
      }

      return throwError(() => error);
    })
  );
};
