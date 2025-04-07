import { Component } from '@angular/core';
import { eventRoutes } from '../../constants/routes';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
	selector: 'app-event-nav',
	imports: [MatIconModule, RouterLink, RouterLinkActive],
	templateUrl: './event-nav.component.html',
	styleUrl: './event-nav.component.scss',
})
export class EventNavComponent {
	eventRoutes = eventRoutes;
}
