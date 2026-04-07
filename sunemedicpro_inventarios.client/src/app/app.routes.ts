import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegistroComponent } from './auth/registro/registro.component';
import { LayoutLandingPageComponent } from './compartidos/layouts/layout-home/layout-landing-page/layout-landing-page.component';
import { LayoutAdministrativoComponent } from './compartidos/layouts/layout-administrativo/layout-administrativo/layout-administrativo.component';
import { HomePrincipalComponent } from './compartidos/layouts/layout-home/home-principal/home-principal.component';

export const routes: Routes = [

  // Rutas de navegación
  //{ path: 'registro', component: RegistroComponent },
  //{ path: 'login', component: LayoutLandingPageComponent, component: LoginComponent },

  {
    path: '',
    component: LayoutLandingPageComponent,
    children: [
      { path: '', component: HomePrincipalComponent },              // Ruta raíz → landing
      { path: 'login', component: LoginComponent },        // Ruta login dentro del layout
      { path: 'registro', component: RegistroComponent }   // Registro dentro del mismo layout
    ]
  },


  // Rutas hijas


  {
    path: '',
    component: LayoutAdministrativoComponent,  // ← Aquí va el layout
    children: [
      {
        path: 'inventario',
        loadChildren: () => import('./negocio/inventario/inventario.routes')
          .then(m => m.INVENTARIO_ROUTES)
      },
      // puedes poner más hijos aquí
    ]
  },


  // Rutas no encontradas, siempre al final
  {path: '**', redirectTo: ''}

];
