import { HttpClient, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { PaginacionDTO } from '../../dtos/PaginacionDTO';
import { Observable } from 'rxjs/internal/Observable';
import { EstudianteCreacionDTO, EstudianteDTO, EstudianteUpdateDTO } from '../../dtos/inventarios/estudianteDTO';
import { construirQueryParams } from '../../compartidos/funciones/construirQueryParams';

@Injectable({
  providedIn: 'root'
})
export class EstudianteService {

  constructor() { }

  private http = inject(HttpClient);
  private urlBase = environment.apiURL + '/Estudiantes'

  public obtenerPaginado(paginacion: PaginacionDTO): Observable<HttpResponse<EstudianteDTO[]>> {
    let queryParametros = construirQueryParams(paginacion);
    return this.http.get<EstudianteDTO[]>(this.urlBase, { params: queryParametros, observe: 'response' });
  }

  public obtenerPorId(id: number): Observable<EstudianteDTO> {
    return this.http.get<EstudianteDTO>(`${this.urlBase}/${id}`)
  }

  public actualizar(id: number, estudiante: EstudianteUpdateDTO): Observable<any> {
    return this.http.put(`${this.urlBase}/${id}`, estudiante)
  }

  public crear(estudiante: EstudianteCreacionDTO): Observable<any> {
    return this.http.post(this.urlBase, estudiante)
  }

  public borrar(id: number): Observable<any> {
    return this.http.delete(`${this.urlBase}/${id}`)
  }

  public getTodos(): Observable<EstudianteDTO[]> {
    return this.http.get<EstudianteDTO[]>(`${this.urlBase}/todos`);
  }

}
