import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { NavComponent } from './nav/nav.component';
import { routes } from '../../constants/routes';

@Component({
	selector: 'app-home',
	imports: [RouterOutlet, MatIconModule, CommonModule, NavComponent],
	templateUrl: './home.component.html',
	styleUrl: './home.component.scss',
})
export class HomeComponent {
	routes = routes;
}
