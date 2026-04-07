import { Component, inject, Input, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-autorizado',
  imports: [],
  templateUrl: './autorizado.component.html',
  styleUrl: './autorizado.component.css'
})
export class AutorizadoComponent  {

  seguridadService = inject(AuthService);
  @Input()
  rol?: string;

  //autorizado = false;

  //ngOnInit() {
  //  this.autorizado = this.estaAutorizado();
  //}

   estaAutorizado(): boolean {
    const rolActual = this.seguridadService.obtenerRol();
    const logueado = this.seguridadService.estaLogueado();
    console.log('Autorizado check:', { rolActual, logueado, requerido: this.rol });

    if (this.rol) {

      return rolActual === this.rol;

    } else {
      return logueado;
    }
  }

}
