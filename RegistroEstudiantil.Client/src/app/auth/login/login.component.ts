import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { extraerErrores, extraerErroresIdentity } from '../../compartidos/funciones/extraerErrores';
import { LoginRequestDTO } from '../../dtos/auth/authDTO';
import { MostrarErroresComponent } from '../../compartidos/mostrar-errores/mostrar-errores.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MostrarErroresComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  formLogin!: FormGroup;
  hidePassword = true;
  errores: string[] = [];

  // Inyecciones modernas con Angular >= 14
  private fb = inject(FormBuilder);
  private seguridadService = inject(AuthService);
  private router = inject(Router);
 
  ngOnInit(): void {
    this.formLogin = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      pass: ['', Validators.required]
    });
  }

  // Habilita el boton solo cuando todos los campos requeridos tienen valor y el formulario es valido.
  puedeEnviarFormulario(): boolean {
    return this.formLogin.valid;
  }
  loguear(): void {
    if (!this.formLogin.valid) {
      this.formLogin.markAllAsTouched();
      return;
    }

    const credenciales = this.formLogin.value as LoginRequestDTO;

    this.seguridadService.login(credenciales).subscribe({
      next: () => {
        this.router.navigate(['/inventario/dashboard']);
      },
      error: err => {
        const errores = extraerErroresIdentity(err);
        this.errores = errores;
      }
    });
  }

  obtenerMensajeErrorEmail(): string {
    const campo = this.formLogin.controls['email'];

    if (campo.hasError('required')) {
      return 'El campo email es requerido';
    }

    if (campo.hasError('email')) {
      return 'El email no es vÃ¡lido';
    }

    return '';
  }

  obtenerMensajeErrorPass(): string {
    const campo = this.formLogin.controls['pass'];

    if (campo.hasError('required')) {
      return 'El campo password es requerido';
    }

    return '';
  }
 
}

