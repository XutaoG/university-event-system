import { Component, inject, OnInit } from '@angular/core';
import { ModalService } from '../../../services/modal/modal.service';
import { EventT, RsoEvent } from '../../../types/event-types';
import { parseDate, parseTime } from '../../../utils';
import { RsoService } from '../../../services/rso/rso.service';

@Component({
	selector: 'app-event-modal-view',
	imports: [],
	templateUrl: './event-modal-view.component.html',
	styleUrls: ['./event-modal-view.component.scss', '../modal.component.scss'],
})
export class EventModalViewComponent implements OnInit {
	private modalService = inject(ModalService);
	private rsoService = inject(RsoService);

	event: EventT | null = null;

	eventDateParsed = '';
	eventTimeParsed = '';
	eventTimeEndParsed = '';

	rsoName: string | null = null;

	ngOnInit() {
		// Subscribe to event behaviorSubject
		this.modalService.event$.subscribe((event) => {
			this.event = event;

			if (event != null) {
				this.eventDateParsed = parseDate(event.eventDate);
				this.eventTimeParsed = parseTime(event.eventTime);
				this.eventTimeEndParsed = parseTime(event.eventTimeEnd);

				// Get RSO Name
				const rsoId = (event as RsoEvent).rsoid;
				if (rsoId != null) {
					// Set RSO Name
					this.rsoService.getRsoById(rsoId).subscribe((rso) => {
						return (this.rsoName = rso.name);
					});
				} else {
					this.rsoName = null;
				}
			}
		});
	}

	onClose() {
		this.modalService.setEvent(null);
		this.modalService.setRso(null);
	}
}
