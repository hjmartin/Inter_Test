export interface EstudianteDTO {
  id: number;
  documento: string;
  nombres: string;
  apellidos: string;
  fechaRegistro: Date;
  usuarioId: number;
}


export interface EstudianteCreacionDTO {
  documento: string;
  nombres: string;
  apellidos: string;
}
export interface EstudianteUpdateDTO {
  documento: string; 
  nombres: string;   
  apellidos: string;
}

