import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { InscripcionService } from '../../../../services/inventarios/inscripcion.service';
import { companierosDto, GrupoInfoDto, InscripcionPublicaDto } from '../../../../dtos/inventarios/inscripcionesDTO';
import { EstudianteDTO } from '../../../../dtos/inventarios/estudianteDTO';
import { forkJoin } from 'rxjs';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import { PaginacionDTO } from '../../../../dtos/PaginacionDTO';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-consultar-companieros-yotros',
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule, MatPaginatorModule, RouterModule],

  templateUrl: './consultar-companieros-yotros.component.html',
  styleUrl: './consultar-companieros-yotros.component.css'
})
export class ConsultarCompanierosYotrosComponent implements OnInit {

  ngOnInit(): void {
    this.cargarDatos();
  }


  //Inyecctar servicios
  private inscripcionService = inject(InscripcionService);
  private estudianteService = inject(EstudianteService);
  //variables


  listaCompanieros: string[] = [];
  estudianteLogueado: EstudianteDTO[] = [];
  estudianteSeleccionado: number | null = null;


  cargarDatos(): void {
    const paginacion: PaginacionDTO = {
      pagina: 1,
      recordsPorPagina: 10
    };

    this.estudianteService.obtenerPaginado(paginacion).subscribe({
      next: (resEstudiante) => {
        // Extraemos la lista de estudiantes del body, usamos array vacío si es null
        const estudiantes: EstudianteDTO[] = resEstudiante.body ?? [];

        if (estudiantes.length === 0) {
          Swal.fire({
            icon: 'warning',
            title: 'Atención',
            text: 'Debe crearse primero como estudiante antes de continuar.',
            confirmButtonText: 'Entendido'
          });
          return;
        }

        // Si hay estudiantes, ejecutamos las demás llamadas concurrentes
        forkJoin({
          companieros: this.inscripcionService.verCompaneros()
          // Puedes agregar más llamadas aquí si necesitas
        }).subscribe({
          next: (res) => {
            this.listaCompanieros = res.companieros;
            // Aquí puedes manejar más resultados si agregaste más llamadas en forkJoin
          },
          error: (err) => {
            console.error('Error al cargar datos en forkJoin:', err);
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'Ocurrió un error al cargar los datos adicionales.',
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
          text: 'Ocurrió un error al obtener los estudiantes.',
          confirmButtonText: 'Aceptar'
        });
      }
    });
  }


    

}
