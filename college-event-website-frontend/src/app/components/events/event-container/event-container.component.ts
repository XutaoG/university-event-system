import { Component, inject, input, OnInit } from '@angular/core';
import { CardContainerComponent } from '../../card-container/card-container.component';
import { EventT } from '../../../types/event-types';
import { ModalService } from '../../../services/modal/modal.service';
import { parseDate, parseTime } from '../../../utils';

@Component({
	selector: 'app-event-container',
	imports: [CardContainerComponent],
	templateUrl: './event-container.component.html',
	styleUrl: './event-container.component.scss',
})
export class EventContainerComponent implements OnInit {
	private modalService = inject(ModalService);

	event = input.required<EventT>();

	eventDateParsed = '';
	eventTimeParsed = '';
	eventTimeEndParsed = '';

	ngOnInit() {
		this.eventDateParsed = parseDate(this.event().eventDate);
		this.eventTimeParsed = parseTime(this.event().eventTime);
		this.eventTimeEndParsed = parseTime(this.event().eventTimeEnd);
	}

	onViewDetail = () => {
		this.modalService.setEvent(this.event());
	};
}
