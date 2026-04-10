import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { InscripcionService } from '../../../../services/inventarios/inscripcion.service';
import { EstudianteDTO } from '../../../../dtos/inventarios/estudianteDTO';
import { forkJoin } from 'rxjs';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-consultar-companieros-yotros',
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, RouterModule],

  templateUrl: './consultar-companieros-yotros.component.html',
  styleUrl: './consultar-companieros-yotros.component.css'
})
export class ConsultarCompanierosYotrosComponent implements OnInit {

  ngOnInit(): void {
    this.cargarDatos();
  }

  private inscripcionService = inject(InscripcionService);
  private estudianteService = inject(EstudianteService);

  listaCompanieros: string[] = [];
  estudianteLogueado: EstudianteDTO[] = [];
  estudianteSeleccionado: number | null = null;

  cargarDatos(): void {
    this.estudianteService.obtenerActual().subscribe({
      next: (estudiantes: EstudianteDTO[]) => {
        if (estudiantes.length === 0) {
          Swal.fire({
            icon: 'warning',
            title: 'Atención',
            text: 'Debe crearse primero como estudiante antes de continuar.',
            confirmButtonText: 'Entendido'
          });
          return;
        }

        forkJoin({
          companieros: this.inscripcionService.verCompaneros()
        }).subscribe({
          next: (res) => {
            this.listaCompanieros = res.companieros;
          },
          error: (err) => {
            console.error('Error al cargar datos en forkJoin:', err);
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'Ocurrio un error al cargar los datos adicionales.',
              confirmButtonText: 'Aceptar'
            });
          }
        });
      },
      error: (err) => {
        console.error('Error al obtener estudiantes:', err);
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Ocurrio un error al obtener los estudiantes.',
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }
}
