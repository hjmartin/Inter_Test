import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-menu',
  imports: [MatButtonModule, MatIconModule, MatToolbarModule, RouterLink],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {

  seguridadService = inject(AuthService);

  emailUsuario: string = 'harold ';
  rol: string = '';
  isMenuOpen = false; // <- para el menú móvil

  ngOnInit(): void {
    this.rol = this.seguridadService.obtenerCampoJWT(
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'
    );
  }
}
