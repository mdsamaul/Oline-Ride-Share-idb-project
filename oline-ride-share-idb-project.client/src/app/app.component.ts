import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderMenuComponent } from "./home/header-menu/header-menu.component";
import { CarouselSliderComponent } from "./home/carousel-slider/carousel-slider.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderMenuComponent, CarouselSliderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  template: `
  <h1>Welcome to Angular!</h1>
  <router-outlet></router-outlet>  <!-- This is where routed views will be displayed -->
`,
})
export class AppComponent {
  title = 'oline-ride-share-idb-project.client';
}
