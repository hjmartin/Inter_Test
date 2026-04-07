import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { GrupoInfoDto, InscripcionCreateDto, InscripcionInfoDto, InscripcionPublicaDto } from '../../dtos/inventarios/inscripcionesDTO';

@Injectable({
  providedIn: 'root'
})
export class InscripcionService {

  constructor() { }

  private http = inject(HttpClient);
  private urlBase = environment.apiURL + '/Inscripciones'

  // GET api/inscripciones
  public getByEstudiante(): Observable<InscripcionInfoDto[]> {
    return this.http.get<InscripcionInfoDto[]>(`${this.urlBase}`);
  }

  public getByOtroEstudiante(estudianteId: number): Observable<InscripcionInfoDto[]> {
    return this.http.get<InscripcionInfoDto[]>(`${this.urlBase}/estudiante/${estudianteId}`);
  }

  // GET api/inscripciones/info
  public getGruposInfo(): Observable<GrupoInfoDto[]> {
    return this.http.get<GrupoInfoDto[]>(`${this.urlBase}/info`);
  }

  public crear(estudiante: InscripcionCreateDto): Observable<any> {
    return this.http.post(this.urlBase, estudiante)
  }

  public borrar(id: number): Observable<any> {
    return this.http.delete(`${this.urlBase}/${id}`)
  }
  // GET para ver compañeros

  verCompaneros(): Observable<string[]> {
    return this.http.get<string[]>(
      `${this.urlBase}/inscripciones-y-companieros`
    );
  }

  verRegistrosDeOtro(estudianteId: number, periodo: string): Observable<InscripcionPublicaDto[]> {
    return this.http.get<InscripcionPublicaDto[]>(
      `${this.urlBase}/de/${estudianteId}/${periodo}`
    );
  }


}
