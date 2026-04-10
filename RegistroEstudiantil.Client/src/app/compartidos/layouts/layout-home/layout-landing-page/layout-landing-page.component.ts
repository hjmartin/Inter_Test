import { Component } from '@angular/core';
import { MenuComponent } from '../menu/menu.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-layout-landing-page',
  imports: [MenuComponent, RouterOutlet],
  templateUrl: './layout-landing-page.component.html',
  styleUrl: './layout-landing-page.component.css'
})
export class LayoutLandingPageComponent {

}
