import { Routes } from '@angular/router';
import { ViewAdministrativoComponent } from '../../compartidos/layouts/layout-administrativo/view-administrativo/view-administrativo.component';
import { PerfilEstudianteComponent } from './estudiante/perfil-estudiante/perfil-estudiante.component';
import { CrearEstudianteComponent } from './estudiante/crear-estudiante/crear-estudiante.component';
import { ActualizarEstudianteComponent } from './estudiante/actualizar-estudiante/actualizar-estudiante.component';
import { ConsultarInscripcionComponent } from './inscripciones/consultar-inscripcion/consultar-inscripcion.component';
import { ConsultarCompanierosYotrosComponent } from './inscripciones/consultar-companieros-yotros/consultar-companieros-yotros.component';
import { ConsultarOtrosComponent } from './inscripciones/consultar-otros/consultar-otros.component';
import { esAdminGuard } from '../../auth/guards/es-admin.guard';

export const INVENTARIO_ROUTES: Routes = [

  { path: 'dashboard', component: ViewAdministrativoComponent, canActivate: [esAdminGuard]},
 
  //Salidas
  { path: 'estudiantes/consultar', component: PerfilEstudianteComponent,canActivate: [esAdminGuard] },
  { path: 'estudiantes/crear', component: CrearEstudianteComponent,canActivate: [esAdminGuard] },
  { path: 'estudiantes/editar/:id', component: ActualizarEstudianteComponent,canActivate: [esAdminGuard] },

  //inscripciones
  { path: 'inscripciones', component: ConsultarInscripcionComponent, canActivate: [esAdminGuard] },
  { path: 'inscripcionesOtros', component: ConsultarCompanierosYotrosComponent, canActivate: [esAdminGuard] },
  { path: 'inscripcionesOtrosRegistros', component: ConsultarOtrosComponent, canActivate: [esAdminGuard] },

  

];
