import { inject } from '@angular/core';
import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { ErrorService } from '../services/ErrorService';

export const errorInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const router = inject(Router);
  const seguridadService = inject(AuthService);
  const errorService = inject(ErrorService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 0:
          errorService.mostrar('No se pudo conectar al servidor.');
          break;

        case 400:
          console.warn('Detalles (400):', error.error);
          break;

        case 401:
          errorService.mostrar('Tu sesion ha expirado.');
          seguridadService.logout();
          router.navigate(['/login']);
          break;

        case 403:
          errorService.mostrar('No tienes permisos para esta accion.');
          break;

        case 404:
          errorService.mostrar('No se encontro el recurso solicitado.');
          break;

        case 409:
          break;

        case 500:
          errorService.mostrar('Error del servidor. Intenta mas tarde.');
          break;

        default:
          errorService.mostrar(error.error?.mensaje ?? 'Ocurrio un error inesperado.');
          console.error('Error no controlado:', error);
          break;
      }

      return throwError(() => error);
    })
  );
};
