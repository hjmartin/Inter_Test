import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
import { extraerErrores } from '../../../../compartidos/funciones/extraerErrores';
import {
  GrupoInfoDto,
  InscripcionCreateDto,
  InscripcionInfoDto
} from '../../../../dtos/inventarios/inscripcionesDTO';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import { InscripcionService } from '../../../../services/inventarios/inscripcion.service';

@Component({
  selector: 'app-consultar-inscripcion',
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './consultar-inscripcion.component.html',
  styleUrl: './consultar-inscripcion.component.css'
})
export class ConsultarInscripcionComponent implements OnInit {
  private inscripcionService = inject(InscripcionService);
  private estudianteService = inject(EstudianteService);

  listaInscripciones: InscripcionInfoDto[] = [];
  listaGrupos: GrupoInfoDto[] = [];
  grupoSeleccionado: number | null = null;

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.estudianteService.obtenerActual().subscribe({
      next: (estudiantes: any[]) => {
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
          inscripciones: this.inscripcionService.getByEstudiante(),
          grupos: this.inscripcionService.getGruposInfo()
        }).subscribe({
          next: (res) => {
            this.listaInscripciones = res.inscripciones;
            this.listaGrupos = res.grupos;
          },
          error: () => {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'Ocurrio un error al cargar las inscripciones o grupos.',
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

  guardarInscripcion(): void {
    if (!this.grupoSeleccionado) {
      Swal.fire({
        icon: 'warning',
        title: 'Seleccione un grupo',
        text: 'Debe seleccionar un grupo de clase antes de guardar.',
        confirmButtonText: 'Entendido'
      });
      return;
    }

    Swal.fire({
      title: '¿Estás seguro?',
      text: 'Se va a registrar la inscripción',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Si­, guardar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (!result.isConfirmed) {
        return;
      }

      const dto: InscripcionCreateDto = {
        grupoClaseId: this.grupoSeleccionado!,
        periodo: '2025-2'
      };

      this.inscripcionService.crear(dto).subscribe({
        next: () => {
          Swal.fire({
            title: '¡Guardado!',
            text: 'La inscripción fue creada correctamente.',
            icon: 'success',
            confirmButtonText: 'Listo'
          }).then(() => {
            this.cargarDatos();
          });
        },
        error: (err) => {
          let mensajes: string[] = [];

          if (err?.error?.errors) {
            mensajes = extraerErrores(err);
          } else if (err?.error?.mensaje) {
            mensajes = [err.error.mensaje];
          } else {
            mensajes = ['Ocurrio un error inesperado.'];
          }

          Swal.fire({
            icon: 'error',
            title: 'Error al guardar',
            html: mensajes.join('<br>')
          });
        }
      });
    });
  }

  eliminarInscripcion(inscripcion: InscripcionInfoDto): void {
    Swal.fire({
      title: '¿Estáss seguro?',
      text: `Â¿Deseas eliminar la inscripción de "${inscripcion.materia}"?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (!result.isConfirmed) {
        return;
      }

      this.inscripcionService.borrar(inscripcion.id).subscribe({
        next: () => {
          Swal.fire('¡Eliminado!', 'La inscripción ha sido eliminada.', 'success');
          this.cargarDatos();
        },
        error: () => {
          Swal.fire('Error', 'Hubo un problema al eliminar la inscripción.', 'error');
        }
      });
    });
  }
}
