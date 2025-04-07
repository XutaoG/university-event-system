import { Component, inject } from '@angular/core';
import { RsoEventsService } from '../../../services/events/rso-events/rso-events.service';
import { RsoEvent } from '../../../types/event-types';
import { VerticalContainerComponent } from '../../vertical-container/vertical-container.component';
import { EventContainerComponent } from '../event-container/event-container.component';
import { sortEventByDate } from '../../../utils';

@Component({
	selector: 'app-upcoming-rso-events',
	imports: [VerticalContainerComponent, EventContainerComponent],
	templateUrl: './upcoming-rso-events.component.html',
	styleUrl: './upcoming-rso-events.component.scss',
})
export class UpcomingRsoEventsComponent {
	private rsoEventService = inject(RsoEventsService);

	rsoEvents: RsoEvent[] = [];

	ngOnInit() {
		this.rsoEventService.getAllRsoEvents().subscribe((rsoEvents) => {
			rsoEvents = sortEventByDate(rsoEvents);
			const splicedArray = rsoEvents.slice(0, 5);
			this.rsoEvents = splicedArray;
		});
	}
}
