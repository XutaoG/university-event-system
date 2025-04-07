import { Component, inject, OnInit } from '@angular/core';
import { PrivateEventsService } from '../../../services/events/private-events/private-events.service';
import { PrivateEvent } from '../../../types/event-types';
import { VerticalContainerComponent } from '../../vertical-container/vertical-container.component';
import { EventContainerComponent } from '../event-container/event-container.component';
import { GridContainerComponent } from '../../grid-container/grid-container.component';

@Component({
	selector: 'app-private-events',
	imports: [EventContainerComponent, GridContainerComponent],
	templateUrl: './private-events.component.html',
	styleUrl: './private-events.component.scss',
})
export class PrivateEventsComponent implements OnInit {
	private privateEventService = inject(PrivateEventsService);

	privateEvents: PrivateEvent[] = [];

	ngOnInit(): void {
		this.privateEventService
			.getAllPrivateEvents()
			.subscribe((privateEvents) => {
				this.privateEvents = privateEvents;
			});
	}
}
