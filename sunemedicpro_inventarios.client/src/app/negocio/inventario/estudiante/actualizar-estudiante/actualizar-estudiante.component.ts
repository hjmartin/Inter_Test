import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { getErrorMensajeNumero, getErrorMensajeTexto, getErrorMensajeEmail, getErrorMensajeSelect } from '../../../../compartidos/funciones/extraerErrores';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import Swal from 'sweetalert2';
import { EstudianteUpdateDTO } from '../../../../dtos/inventarios/estudianteDTO';

@Component({
  selector: 'app-actualizar-estudiante',
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterLink],
  templateUrl: './actualizar-estudiante.component.html',
  styleUrl: './actualizar-estudiante.component.css'
})
export class ActualizarEstudianteComponent implements OnInit {

  //inyeccion
  private fb = inject(FormBuilder);
  private estudianteService = inject(EstudianteService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);


  idProducto!: number;

  getErrorMensajeNumero = getErrorMensajeNumero;
  getErrorMensajeTexto = getErrorMensajeTexto;
  getErrorMensajeEmail = getErrorMensajeEmail;
  getErrorMensajeSelect = getErrorMensajeSelect;

  //iniciamos datos en vista
  ngOnInit() {
    this.idProducto = Number(this.route.snapshot.paramMap.get('id'));
    this.cargarEstudiantePorId();
  }
  

  // Creamos el formulario con validadores
  formulario = this.fb.group({
    documento: [
      '',
      [
        Validators.required,
        Validators.maxLength(20) // como en el DTO C# original
      ]
    ],
    nombres: [
      '',
      [
        Validators.required,
        Validators.maxLength(100)
      ]
    ],
    apellidos: [
      '',
      [
        Validators.required,
        Validators.maxLength(100)
      ]
    ]
  });

  private cargarEstudiantePorId(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.estudianteService.obtenerPorId(id).subscribe({
      next: (estudiante) => {
        this.formulario.patchValue({
          documento: estudiante.documento,
          nombres: estudiante.nombres,
          apellidos: estudiante.apellidos,
        });
      },
      error: (error) => {
        console.error('❌ Error al obtener producto por ID', error);
      }
    });
  }


  // Método para guardar el formulario
  guardar() {
    if (this.formulario.valid) {
      Swal.fire({
        title: '¿Estás seguro?',
        text: '¿Deseas guardar los cambios del producto?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, guardar',
        cancelButtonText: 'Cancelar'
      }).then((result) => {
        if (result.isConfirmed) {
          const estudiante: EstudianteUpdateDTO = {
            documento: this.formulario.value.documento || '',
            nombres: this.formulario.value.nombres || '',
            apellidos: this.formulario.value.apellidos || '',
          };

          this.estudianteService.actualizar(this.idProducto, estudiante).subscribe({
            next: () => {

              // ✅ Mostrar alerta y esperar a que el usuario le dé OK
              Swal.fire({
                title: '¡Guardado!',
                text: 'El producto fue actualizado correctamente.',
                icon: 'success',
                confirmButtonText: 'OK'
              }).then(() => {
                // ✅ Navegar después de que el usuario presione OK
                this.router.navigate(['/inventario/estudiantes/consultar']);
              });

            },
            error: (error) => {
              console.error('❌ Error al actualizar producto', error);
              Swal.fire('Error', 'Ocurrió un error al guardar los cambios.', 'error');
            }
          });
        }
      });
    } else {
      this.formulario.markAllAsTouched();
    }
  }
}
