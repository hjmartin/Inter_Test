export interface LoginResponseDTO {
  token: string;
  usuarioId: number;
  nombre: string;
  correo: string;
  expiracion: Date;
}

export interface LoginRequestDTO {
  email: string;
  pass: string; 
}

export interface UsuarioDTO {
  email: string;
}

export interface RegisterDto {
  email: string;
  pass: string;
  documento: string;
  nombres: string;
  apellidos: string;
}

