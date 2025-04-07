import { Component, inject, OnInit } from '@angular/core';
import { PublicEvent } from '../../../types/event-types';
import { PublicEventsService } from '../../../services/events/public-events/public-events.service';
import { VerticalContainerComponent } from '../../vertical-container/vertical-container.component';
import { EventContainerComponent } from '../event-container/event-container.component';
import { GridContainerComponent } from '../../grid-container/grid-container.component';

@Component({
	selector: 'app-public-events',
	imports: [EventContainerComponent, GridContainerComponent],
	templateUrl: './public-events.component.html',
	styleUrl: './public-events.component.scss',
})
export class PublicEventsComponent implements OnInit {
	private publicEventService = inject(PublicEventsService);

	publicEvents: PublicEvent[] = [];

	ngOnInit() {
		this.publicEventService
			.getAllPublicEvents()
			.subscribe((publicEvents) => {
				this.publicEvents = publicEvents;
			});
	}
}
