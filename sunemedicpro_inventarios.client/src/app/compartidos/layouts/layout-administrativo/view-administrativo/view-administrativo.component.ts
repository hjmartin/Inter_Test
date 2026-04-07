import { Component, OnInit, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Observable } from 'rxjs';
import { GraficasDTO } from '../../../../dtos/shared/sharedDTO';

@Component({
  selector: 'app-view-administrativo',
  imports: [MatCardModule],
  templateUrl: './view-administrativo.component.html',
  styleUrl: './view-administrativo.component.css'
})
export class ViewAdministrativoComponent  {

  iniciostr: string = "";
  finalstr: string = "";
  grafica1: GraficasDTO[] = [];
  grafica2: GraficasDTO[] = [];
  grafica3: GraficasDTO[] = [];

  //ngOnInit(): void {
  //  this.cargarConsultas();
  //}

}
