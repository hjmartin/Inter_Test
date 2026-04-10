import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import { EstudianteDTO } from '../../../../dtos/inventarios/estudianteDTO';
import { catchError, debounceTime, distinctUntilChanged, map, merge, of, startWith, Subject, switchMap, takeUntil } from 'rxjs';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-perfil-estudiante',
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './perfil-estudiante.component.html',
  styleUrl: './perfil-estudiante.component.css'
})
export class PerfilEstudianteComponent implements OnInit, OnDestroy {
  ngOnInit(): void {
    this.CargarEstudiantes();
  }

  private estudianteService = inject(EstudianteService);

  listaEstudiantes: EstudianteDTO[] = [];
  searchTerm: string = '';

  private filtro$ = new Subject<string>();
  private destroy$ = new Subject<void>();

  CargarEstudiantes() {
    merge(
      of(null).pipe(startWith(null)),
      this.filtro$.pipe(
        map(v => (v ?? '').trim()),
        debounceTime(600),
        distinctUntilChanged()
      )
    ).pipe(
      switchMap(() => {
        const filtro = (this.searchTerm ?? '').trim().toLowerCase();

        return this.estudianteService.obtenerActual().pipe(
          map(estudiantes => {
            if (!filtro) {
              return estudiantes;
            }

            return estudiantes.filter(estudiante =>
              `${estudiante.nombres} ${estudiante.apellidos} ${estudiante.documento}`
                .toLowerCase()
                .includes(filtro)
            );
          }),
          catchError(err => {
            console.error('Error al obtener entradas', err);
            return of([] as EstudianteDTO[]);
          })
        );
      }),
      takeUntil(this.destroy$)
    )
      .subscribe((estudiantes) => {
        this.listaEstudiantes = estudiantes;
      });
  }

  onSearchChange(): void {
    this.filtro$.next(this.searchTerm);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  eliminarEstudiante(estudiante: EstudianteDTO): void {
    Swal.fire({
      title: '¿Estás seguro?',
      text: `¿Deseas eliminar el estudainte "${estudiante.nombres}"?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si­, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.estudianteService.borrar(estudiante.id).subscribe({
          next: () => {
            Swal.fire('¡Eliminado!', 'El estudiante ha sido eliminado.', 'success');
            this.CargarEstudiantes();
          },
          error: () => {
            Swal.fire('Error', 'Hubo un problema al eliminar el estudiante, porque ya tiene registro de materias.', 'error');
          }
        });
      }
    });
  }
}
