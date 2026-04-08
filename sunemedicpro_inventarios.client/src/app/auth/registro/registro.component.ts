import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { RegisterDto } from '../../dtos/auth/authDTO';
import { extraerErrores, extraerErroresIdentity } from '../../compartidos/funciones/extraerErrores';
import { MostrarErroresComponent } from '../../compartidos/mostrar-errores/mostrar-errores.component';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MostrarErroresComponent],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent implements OnInit {
  formRegistro!: FormGroup;
  hidePassword = true;
  errores: string[] = [];

  private fb = inject(FormBuilder);
  private seguridadService = inject(AuthService);
  private router = inject(Router);

  ngOnInit(): void {
    this.formRegistro = this.fb.group({
      documento: ['', [Validators.required, Validators.maxLength(20)]],
      nombres: ['', [Validators.required, Validators.maxLength(100)]],
      apellidos: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      pass: ['', Validators.required]
    });
  }

  registrar(): void {
    if (!this.formRegistro.valid) {
      this.formRegistro.markAllAsTouched();
      return;
    }

    this.limpiarErroresApi();
    this.errores = [];

    const datos = this.formRegistro.getRawValue() as RegisterDto;
    this.seguridadService.logout(false);

    this.seguridadService.register(datos).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Registro completado con exito',
          text: 'Tu cuenta y tu perfil de estudiante fueron creados correctamente.',
          confirmButtonText: 'Aceptar'
        }).then(() => {
          this.router.navigate(['/inventario/dashboard']);
        });
      },
      error: err => {
        if (err?.error?.errors) {
          const mensajes = extraerErrores(err);
          this.asignarErroresPorCampo(mensajes);
        } else if (err?.error?.mensaje) {
          this.asignarErroresPorCampo([err.error.mensaje]);
        } else if (Array.isArray(err?.error)) {
          this.errores = extraerErroresIdentity(err);
        } else {
          this.errores = ['Ocurrio un error inesperado.'];
        }
      }
    });
  }

  obtenerMensajeErrorDocumento(): string {
    const campo = this.formRegistro.controls['documento'];

    if (campo.hasError('api')) {
      return campo.getError('api');
    }

    if (campo.hasError('required')) {
      return 'El documento es requerido';
    }

    if (campo.hasError('maxlength')) {
      return 'El documento no puede superar los 20 caracteres';
    }

    return '';
  }

  obtenerMensajeErrorNombres(): string {
    const campo = this.formRegistro.controls['nombres'];

    if (campo.hasError('api')) {
      return campo.getError('api');
    }

    if (campo.hasError('required')) {
      return 'Los nombres son requeridos';
    }

    if (campo.hasError('maxlength')) {
      return 'Los nombres no pueden superar los 100 caracteres';
    }

    return '';
  }

  obtenerMensajeErrorApellidos(): string {
    const campo = this.formRegistro.controls['apellidos'];

    if (campo.hasError('api')) {
      return campo.getError('api');
    }

    if (campo.hasError('required')) {
      return 'Los apellidos son requeridos';
    }

    if (campo.hasError('maxlength')) {
      return 'Los apellidos no pueden superar los 100 caracteres';
    }

    return '';
  }

  obtenerMensajeErrorEmail(): string {
    const campo = this.formRegistro.controls['email'];

    if (campo.hasError('api')) {
      return campo.getError('api');
    }

    if (campo.hasError('required')) {
      return 'El campo email es requerido';
    }

    if (campo.hasError('email')) {
      return 'El email no es valido';
    }

    return '';
  }

  obtenerMensajeErrorPass(): string {
    const campo = this.formRegistro.controls['pass'];

    if (campo.hasError('api')) {
      return campo.getError('api');
    }

    if (campo.hasError('required')) {
      return 'El campo password es requerido';
    }

    return '';
  }

  private limpiarErroresApi(): void {
    Object.keys(this.formRegistro.controls).forEach(nombreControl => {
      const control = this.formRegistro.get(nombreControl);
      if (!control?.errors?.['api']) {
        return;
      }

      const { api, ...resto } = control.errors;
      control.setErrors(Object.keys(resto).length ? resto : null);
    });
  }

  private asignarErroresPorCampo(mensajes: string[]): void {
    const erroresGenerales: string[] = [];

    mensajes.forEach(mensaje => {
      const mensajeNormalizado = mensaje.toLowerCase();

      if (mensajeNormalizado.includes('documento')) {
        this.formRegistro.controls['documento'].setErrors({
          ...(this.formRegistro.controls['documento'].errors ?? {}),
          api: mensaje
        });
        this.formRegistro.controls['documento'].markAsTouched();
        return;
      }

      if (mensajeNormalizado.includes('email') || mensajeNormalizado.includes('correo')) {
        this.formRegistro.controls['email'].setErrors({
          ...(this.formRegistro.controls['email'].errors ?? {}),
          api: mensaje
        });
        this.formRegistro.controls['email'].markAsTouched();
        return;
      }

      erroresGenerales.push(mensaje);
    });

    this.errores = erroresGenerales;
  }
}
