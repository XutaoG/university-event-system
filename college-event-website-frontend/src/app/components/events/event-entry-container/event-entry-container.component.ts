import { Component, inject, input } from '@angular/core';
import { CardContainerComponent } from '../../card-container/card-container.component';
import { ModalService } from '../../../services/modal/modal.service';
import { EventT } from '../../../types/event-types';
import { parseDate, parseTime } from '../../../utils';

@Component({
	selector: 'app-event-entry-container',
	imports: [CardContainerComponent],
	templateUrl: './event-entry-container.component.html',
	styleUrl: './event-entry-container.component.scss',
})
export class EventEntryContainerComponent {
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
