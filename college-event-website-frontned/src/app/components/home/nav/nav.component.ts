import { Component, input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { routes } from '../../../constants/routes';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'app-nav',
	imports: [RouterLink, RouterLinkActive, MatIconModule],
	templateUrl: './nav.component.html',
	styleUrl: './nav.component.scss',
})
export class NavComponent {
	routes = routes;
}
