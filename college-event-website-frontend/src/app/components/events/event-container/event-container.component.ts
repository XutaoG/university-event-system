import { Component, input, OnInit } from '@angular/core';
import { CardContainerComponent } from '../../card-container/card-container.component';
import { EventT } from '../../../types/event-types';

@Component({
	selector: 'app-event-container',
	imports: [CardContainerComponent],
	templateUrl: './event-container.component.html',
	styleUrl: './event-container.component.scss',
})
export class EventContainerComponent implements OnInit {
	event = input.required<EventT>();

	eventDateParsed = '';
	eventTimeParsed = '';
	eventTimeEndParsed = '';

	// eventTimeEndParsed = new Date(this.event_().eventTimeEnd).getHours;

	ngOnInit() {
		// Parse date
		const eventDate = new Date(this.event().eventDate);
		this.eventDateParsed = `${
			eventDate.getMonth() + 1
		}/${eventDate.getDay()}/${eventDate
			.getFullYear()
			.toString()
			.substring(2)}`;

		// Parse event start time
		const eventTime = new Date(
			'1970-01-01T' + this.event().eventTime + 'Z'
		);

		this.eventTimeParsed = new Date(
			'1970-01-01T' + this.event().eventTime + 'Z'
		).toLocaleString('en-US', {
			hour: 'numeric',
			minute: 'numeric',
			hour12: true,
		});

		this.eventTimeEndParsed = new Date(
			'1970-01-01T' + this.event().eventTimeEnd + 'Z'
		).toLocaleString('en-US', {
			hour: 'numeric',
			minute: 'numeric',
			hour12: true,
		});
	}
}
