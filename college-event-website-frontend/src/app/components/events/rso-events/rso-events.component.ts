import { Component, inject, OnInit, signal } from '@angular/core';
import { RsoEventsService } from '../../../services/events/rso-events/rso-events.service';
import { VerticalContainerComponent } from '../../vertical-container/vertical-container.component';
import { EventContainerComponent } from '../event-container/event-container.component';
import { RsoEvent } from '../../../types/event-types';
import { GridContainerComponent } from '../../grid-container/grid-container.component';

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
			this.rsoEvents = rsoEvents;
		});
	}
}
