import { Component, OnInit, inject } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { RouterOutlet, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

// Angular Material
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatExpansionModule } from '@angular/material/expansion';

// Otros módulos/componentes
import { FlexLayoutModule } from '@angular/flex-layout';
import { HeaderAdministrativoComponent } from '../header-administrativo/header-administrativo.component';

// Servicio de autenticación
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-layout-administrativo',
  standalone: true,
  imports: [CommonModule,
    RouterOutlet,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatExpansionModule,
    FlexLayoutModule,
    HeaderAdministrativoComponent,
    RouterLink],
  templateUrl: './layout-administrativo.component.html',
  styleUrl: './layout-administrativo.component.css'
})
export class LayoutAdministrativoComponent implements OnInit {

  seguridadService = inject(AuthService);

  isMobile = false;
  correo = "";

  constructor(private breakpointObserver: BreakpointObserver) { }

  ngOnInit(): void {

    this.correo = this.seguridadService.obtenerCampoJWT(
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
    );

    this.breakpointObserver.observe([Breakpoints.Handset])
      .subscribe(result => {
        this.isMobile = result.matches;
      });

  }

  mostrarDiv = true;

  opcionActiva: string = '';

  activarElemento(opcion: string): void {
    this.opcionActiva = opcion;
  }
}
