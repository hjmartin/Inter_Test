export interface InscripcionInfoDto {
  id: number;
  nombreGrupo: string;
  materia: string;
  profesor: string;
  creditos: number;
}

export interface GrupoInfoDto {
  id: number;
  grupoInfo: string;
}

export interface InscripcionCreateDto {
  grupoClaseId: number;
  periodo: string;
}

export interface InscripcionPublicaDto {
  materia: string;
  periodo: string;
}

export interface companierosDto {
  nombre: string;
  periodo: string;
}
