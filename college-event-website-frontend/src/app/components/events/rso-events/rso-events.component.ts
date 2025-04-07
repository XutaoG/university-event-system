import { Component, inject, OnInit } from '@angular/core';
import { RsoEventsService } from '../../../services/events/rso-events/rso-events.service';
import { EventContainerComponent } from '../event-container/event-container.component';
import { RsoEvent } from '../../../types/event-types';
import { GridContainerComponent } from '../../grid-container/grid-container.component';
import { sortEventByDate } from '../../../utils';

@Component({
	selector: 'app-rso-events',
	imports: [EventContainerComponent, GridContainerComponent],
	templateUrl: './rso-events.component.html',
	styleUrl: './rso-events.component.scss',
})
export class RsoEventsComponent implements OnInit {
	private rsoEventService = inject(RsoEventsService);

	rsoEvents: RsoEvent[] = [];

	ngOnInit() {
		this.rsoEventService.getAllRsoEvents().subscribe((rsoEvents) => {
			sortEventByDate(rsoEvents);
			this.rsoEvents = rsoEvents;
		});
	}
}
