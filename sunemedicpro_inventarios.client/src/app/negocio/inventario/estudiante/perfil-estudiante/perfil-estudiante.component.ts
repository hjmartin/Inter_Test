import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import { EstudianteDTO } from '../../../../dtos/inventarios/estudianteDTO';
import { catchError, debounceTime, distinctUntilChanged, map, merge, of, startWith, Subject, switchMap, takeUntil, tap } from 'rxjs';
import { PaginacionDTO } from '../../../../dtos/PaginacionDTO';
import { HttpResponse } from '@angular/common/http';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-perfil-estudiante',
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, MatPaginatorModule, RouterModule],
  templateUrl: './perfil-estudiante.component.html',
  styleUrl: './perfil-estudiante.component.css'
})
export class PerfilEstudianteComponent implements OnInit {
  ngOnInit(): void {
    this.CargarEstudiantes();
    }

  //Inyecctar servicios
  private estudianteService = inject(EstudianteService);

  //variables
  totalRecords = 0;
  recordsPorPagina = 10;
  paginaActual = 1;
  listaEstudiantes: EstudianteDTO[] = [];
  searchTerm: string = '';

  // triggers reactivos
  private filtro$ = new Subject<string>();           // cambios en input de búsqueda
  private pagina$ = new Subject<{ page: number; size: number }>(); // cambios en paginación
  private destroy$ = new Subject<void>();

  CargarEstudiantes() {
    // flujo combinado: carga inicial + cambios de filtro + cambios de paginación
    merge(
      // 1) CARGA INICIAL
      of(null).pipe(startWith(null)),
      // 2) cambios de filtro (usa this.searchTerm)
      this.filtro$.pipe(
        map(v => (v ?? '').trim()),
        debounceTime(600),
        distinctUntilChanged(),
        tap(() => this.paginaActual = 1) // si cambias el filtro, vuelve a la página 1
      ),
      // 3) cambios de paginación
      this.pagina$.pipe(
        tap(v => { this.paginaActual = v.page; this.recordsPorPagina = v.size; })
      )
    ).pipe(
      switchMap(() => {
        const paginacion: PaginacionDTO = {
          pagina: this.paginaActual,
          recordsPorPagina: this.recordsPorPagina
        };
        const filtro = (this.searchTerm ?? '').trim();
        const filtroOpcional = filtro ? filtro : undefined;

        return this.estudianteService.obtenerPaginado(paginacion).pipe(
          catchError(err => {
            console.error('Error al obtener entradas', err);
            return of({ body: [], headers: new Map() } as unknown as HttpResponse<EstudianteDTO[]>);
          })
        );
      }),
      takeUntil(this.destroy$)
    )
      .subscribe((response) => {
        this.listaEstudiantes = response.body ?? [];
        const total = response.headers.get('cantidad-total-registros');
        this.totalRecords = total ? +total : this.listaEstudiantes.length;
      });
  }

  // 🔎 disparar filtro desde el input
  onSearchChange(): void {
    this.filtro$.next(this.searchTerm);
  }

  // 📄 paginación
  onPageChange(event: PageEvent) {
    this.recordsPorPagina = event.pageSize;
    this.paginaActual = event.pageIndex + 1;
    this.pagina$.next({ page: this.paginaActual, size: this.recordsPorPagina });
  }

  // ♻️ limpieza
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


  eliminarEstudiante(estudiante: EstudianteDTO): void {

    //  Swal.fire({
    //    title: "Custom width, padding, color, background.",
    //    width: 600,
    //    padding: "3em",
    //    color: "#716add",
    //    background: "#fff url(/images/trees.png)",
    //    backdrop: `
    //  rgba(0,0,123,0.4)
    //  url("https://i.gifer.com/6mg.gif")
    //  left top
    //  no-repeat
    //`
    //  });

    Swal.fire({
      title: '¿Estás seguro?',
      text: `¿Deseas eliminar el estudainte "${estudiante.nombres}"?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.estudianteService.borrar(estudiante.id).subscribe({
          next: () => {
            Swal.fire('¡Eliminado!', 'El estudiante ha sido eliminado.', 'success');
            // Recargar la lista de productos
            this.CargarEstudiantes();
          },
          error: () => {
            Swal.fire('Error', 'Hubo un problema al eliminar el estudiante.', 'error');
          }
        });
      }
    });
  }


}
