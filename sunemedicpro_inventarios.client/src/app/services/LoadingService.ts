// ✅ Servicio: loading.service.ts
import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private _cargando = signal(false);
  public cargando = this._cargando.asReadonly();

  mostrar() {
    this._cargando.set(true);
  }

  ocultar() {
    this._cargando.set(false);
  }
}
