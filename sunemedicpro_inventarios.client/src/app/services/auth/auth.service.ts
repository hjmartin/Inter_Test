import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { LoginRequestDTO, LoginResponseDTO, RegisterDto } from '../../dtos/auth/authDTO';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  private http = inject(HttpClient);
  private router = inject(Router);
  private urlBase = environment.apiURL + '/usuarios';

  private readonly llaveToken = 'token';
  private readonly llaveExpiracion = 'token-expiracion';


  obtenerToken(): string | null {
    return localStorage.getItem(this.llaveToken);
  }

  login(credenciales: LoginRequestDTO): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${this.urlBase}/login`, credenciales)
      .pipe(
        tap(resp => this.guardarToken(resp))

      )
  }

  guardarToken(RespuestaAutenticacionDTO: LoginResponseDTO) {
    localStorage.setItem(this.llaveToken, RespuestaAutenticacionDTO.token);
    localStorage.setItem(this.llaveExpiracion, RespuestaAutenticacionDTO.expiracion.toString());
  }

  obtenerCampoJWT(campo: string): string {
    const token = localStorage.getItem(this.llaveToken);
    if (!token) { return '' }

    var dataToken = JSON.parse(atob(token.split('.')[1]))

    return dataToken[campo];
  }

  estaLogueado(): boolean {
    const token = localStorage.getItem(this.llaveToken);

    if (!token) {
      return false;
    }

    const expiracion = localStorage.getItem(this.llaveExpiracion)!;
    const expriacionFecha = new Date(expiracion);

    if (expriacionFecha <= new Date()) {
      this.logout();
      return false;
    }

    return true;
  }

  logout(redirigir: boolean = true) {
    localStorage.removeItem(this.llaveToken);
    localStorage.removeItem(this.llaveExpiracion);

    if (redirigir) {
      this.router.navigate(['']);
    }
  }

  obtenerRol(): string {
    const esAdmin = this.obtenerCampoJWT('http://schemas.microsoft.com/ws/2008/06/identity/claims/role');
    if (esAdmin) {
      return esAdmin;
    } else {
      return '';
    }
  }

  register(dto: RegisterDto): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(`${this.urlBase}/register`, dto)
      .pipe(
        tap(resp => this.guardarToken(resp))
      );
  }

}
