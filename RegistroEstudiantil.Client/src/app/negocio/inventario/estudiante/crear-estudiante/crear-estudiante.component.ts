import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { getErrorMensajeNumero, getErrorMensajeTexto, getErrorMensajeEmail, getErrorMensajeSelect, extraerErrores } from '../../../../compartidos/funciones/extraerErrores';
import { Router } from '@angular/router';
import { EstudianteService } from '../../../../services/inventarios/estudiante.service';
import Swal from 'sweetalert2';
import { EstudianteCreacionDTO } from '../../../../dtos/inventarios/estudianteDTO';


@Component({
  selector: 'app-crear-estudiante',
  imports: [CommonModule, ReactiveFormsModule, FlexLayoutModule, MatCardModule], 
  templateUrl: './crear-estudiante.component.html',
  styleUrl: './crear-estudiante.component.css'
})
export class CrearEstudianteComponent {

  //inyectar dependencias
  private fb = inject(FormBuilder);
  private router = inject(Router); // âœ… inyectamos Router
  private estudianteService = inject(EstudianteService);

  //funciones globales para errores
  getErrorMensajeNumero = getErrorMensajeNumero;
  getErrorMensajeTexto = getErrorMensajeTexto;
  getErrorMensajeEmail = getErrorMensajeEmail;
  getErrorMensajeSelect = getErrorMensajeSelect;

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


  //funciones

  // Habilita el boton solo cuando todos los campos requeridos tienen valor y el formulario es valido.
  puedeEnviarFormulario(): boolean {
    return this.formulario.valid;
  }
  // MÃ©todo para guardar el formulario
  guardar() {
    if (this.formulario.valid) {
      Swal.fire({
        title: '¿Estás seguro?',
        text: 'Se va a crear el estudiante',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Si­, guardar',
        cancelButtonText: 'Cancelar'
      }).then((result) => {
        if (result.isConfirmed) {
          const dto: EstudianteCreacionDTO = {
            documento: this.formulario.value.documento!,
            nombres: this.formulario.value.nombres!,
            apellidos: this.formulario.value.apellidos!

          };

          this.estudianteService.crear(dto).subscribe({
            next: () => {
              Swal.fire({
                title: '¡Guardado!',
                text: 'El estudiante fue creado correctamente.',
                icon: 'success',
                confirmButtonText: 'Listo'
              }).then((result) => {
                if (result.isConfirmed) {
                  this.router.navigate(['/inventario/estudiantes/consultar']);
                }
              });
            },
            error: (err) => {
              let mensajes: string[] = [];

              if (err?.error?.errors) {
                // Errores de validaciÃ³n de ModelState
                mensajes = extraerErrores(err);
              } else if (err?.error?.mensaje) {
                // Errores de negocio, ej. 409 Conflict
                mensajes = [err.error.mensaje];
              } else {
                mensajes = ['OcurriÃ³ un error inesperado.'];
              }

              Swal.fire({
                icon: 'error',
                title: 'âŒ Error al guardar',
                html: mensajes.join('<br>')
              });
            }
          });
        }
      });

    } else {
      this.formulario.markAllAsTouched(); // Marca todos los campos como tocados
    }
  }
}


