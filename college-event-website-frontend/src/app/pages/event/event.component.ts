import { Component } from '@angular/core';
import { PublicEventsComponent } from '../../components/events/public-events/public-events.component';
import { PrivateEventsComponent } from '../../components/events/private-events/private-events.component';
import { RsoEventsComponent } from '../../components/events/rso-events/rso-events.component';

@Component({
	selector: 'app-event',
	imports: [
		PublicEventsComponent,
		PrivateEventsComponent,
		RsoEventsComponent,
	],
	templateUrl: './event.component.html',
	styleUrl: './event.component.scss',
})
export class EventComponent {}
