import { Component, inject } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-header-administrativo',
  imports: [
    MatMenuModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './header-administrativo.component.html',
  styleUrl: './header-administrativo.component.css'
})
export class HeaderAdministrativoComponent {
  seguridadService = inject(AuthService);
}
