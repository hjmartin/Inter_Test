import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

export const esAdminGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);
  const seguridadService = inject(AuthService);

  if (seguridadService.obtenerRol() === 'Estudiante') {
    return true;
  }

  router.navigate(['/login']);
  return true;


};
