import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoadingService } from '../../services/LoadingService';


@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule],
  template: `
    <div *ngIf="loadingService.cargando()" class="overlay">
      <mat-spinner diameter="50"></mat-spinner>
    </div>
  `,
  styles: [
    `
    .overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100vw;
      height: 100vh;
      background-color: rgba(255, 255, 255, 0.5);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 9999;
    }
    `
  ]
})
export class LoadingComponent {
  constructor(public loadingService: LoadingService) { }
}
