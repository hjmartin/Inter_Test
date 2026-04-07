import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MostrarErroresComponent } from '../../compartidos/mostrar-errores/mostrar-errores.component';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { extraerErroresIdentity } from '../../compartidos/funciones/extraerErrores';
import { RegisterDto } from '../../dtos/auth/authDTO';
import Swal from 'sweetalert2';

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

  // Inyecciones modernas con Angular >= 14
  private fb = inject(FormBuilder);
  private seguridadService = inject(AuthService);
  private router = inject(Router);

  ngOnInit(): void {
    this.formRegistro = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      pass: ['', Validators.required]
    });
  }

  registrar(): void {
    if (!this.formRegistro.valid) {
      this.formRegistro.markAllAsTouched();
      return;
    }

    const datos = this.formRegistro.value as RegisterDto;

    // Primero cerramos la sesión anterior
    this.seguridadService.logout();

    // Registramos al usuario
    this.seguridadService.register(datos).subscribe({
      next: () => {
        // Después de registrar, hacemos login automático
        const credenciales = {
          email: datos.email,      // o el campo que uses para login
          pass: datos.pass // asegúrate de tener la contraseña
        };

        this.seguridadService.login(credenciales).subscribe({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Registrado y logueado con éxito',
              confirmButtonText: 'Aceptar'
            }).then(() => {
              this.router.navigate(['/inventario/dashboard']);
            });
          },
          error: loginErr => {
            const errores = extraerErroresIdentity(loginErr);
            this.errores = errores;
          }
        });
      },
      error: err => {
        const errores = extraerErroresIdentity(err);
        this.errores = errores;
      }
    });
  }


  obtenerMensajeErrorEmail(): string {
    const campo = this.formRegistro.controls['email'];

    if (campo.hasError('required')) {
      return 'El campo email es requerido';
    }

    if (campo.hasError('email')) {
      return 'El email no es válido';
    }

    return '';
  }

  obtenerMensajeErrorPass(): string {
    const campo = this.formRegistro.controls['pass'];

    if (campo.hasError('required')) {
      return 'El campo password es requerido';
    }

    return '';
  }
}
