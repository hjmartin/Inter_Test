import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
import { EstudianteDTO } from '../../../../dtos/inventarios/estudianteDTO';
import { InscripcionInfoDto } from '../../../../dtos/inventarios/inscripcionesDTO';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import { InscripcionService } from '../../../../services/inventarios/inscripcion.service';

@Component({
  selector: 'app-consultar-otros',
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './consultar-otros.component.html',
  styleUrl: './consultar-otros.component.css'
})
export class ConsultarOtrosComponent implements OnInit {
  private inscripcionService = inject(InscripcionService);
  private estudianteService = inject(EstudianteService);

  listaOtrasInscripciones: InscripcionInfoDto[] = [];
  listaEstudiantes: EstudianteDTO[] = [];
  estudianteLogueado: EstudianteDTO | null = null;
  estudianteSeleccionado: number | null = null;

  ngOnInit(): void {
    this.cargarDatos();
  }

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

        this.estudianteLogueado = estudiantes[0];

        forkJoin({
          listaEstudiantes: this.estudianteService.getTodos()
        }).subscribe({
          next: (res) => {
            this.listaEstudiantes = res.listaEstudiantes
              .filter(estudiante => estudiante.id !== this.estudianteLogueado?.id)
              .sort((a, b) =>
                `${a.nombres} ${a.apellidos}`.localeCompare(`${b.nombres} ${b.apellidos}`)
              );
          },
          error: () => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'Ocurrio un error al cargar la lista de estudiantes.',
              confirmButtonText: 'Aceptar'
            });
          }
        });
      },
      error: () => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Ocurrio un error al obtener los estudiantes.',
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }

  cargarDatosOtros(): void {
    if (this.estudianteSeleccionado === null) {
      this.listaOtrasInscripciones = [];
      return;
    }

    this.inscripcionService.getByOtroEstudiante(this.estudianteSeleccionado).subscribe({
      next: (inscripciones) => {
        this.listaOtrasInscripciones = inscripciones;

        if (inscripciones.length === 0) {
          Swal.fire({
            icon: 'info',
            title: 'Sin registros',
            text: 'El estudiante seleccionado no tiene inscripciones registradas.',
            confirmButtonText: 'Aceptar'
          });
        }
      },
      error: (err) => {
        this.listaOtrasInscripciones = [];

        const mensaje = err?.error?.mensaje ?? 'Ocurrio un error al consultar los registros del estudiante.';
        Swal.fire({
          icon: 'error',
          title: 'Error al consultar',
          text: mensaje,
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }

  get nombreEstudianteSeleccionado(): string {
    const estudiante = this.listaEstudiantes.find(x => x.id === this.estudianteSeleccionado);
    return estudiante ? `${estudiante.nombres} ${estudiante.apellidos}` : 'otro estudiante';
  }
}
