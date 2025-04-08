import { Component, inject, OnInit } from '@angular/core';
import { Rso } from '../../types/rso-types';
import { ModalService } from '../../services/modal/modal.service';
import { EventT, RsoEvent } from '../../types/event-types';
import { parseDate, parseTime, sortEventByDate } from '../../utils';
import { RsoService } from '../../services/rso/rso.service';
import { CommonModule } from '@angular/common';
import { RsoEventsService } from '../../services/events/rso-events/rso-events.service';
import { EventEntryContainerComponent } from '../events/event-entry-container/event-entry-container.component';
import { ListContainerComponent } from '../list-container/list-container.component';

@Component({
	selector: 'app-modal',
	imports: [
		CommonModule,
		EventEntryContainerComponent,
		ListContainerComponent,
	],
	templateUrl: './modal.component.html',
	styleUrl: './modal.component.scss',
})
export class ModalComponent implements OnInit {
	private modalService = inject(ModalService);
	private rsoService = inject(RsoService);
	private rsoEventService = inject(RsoEventsService);

	event: EventT | null = null;
	rso: Rso | null = null;

	eventDateParsed = '';
	eventTimeParsed = '';
	eventTimeEndParsed = '';

	rsoName: string | null = null;

	rsoOwned: boolean | null = null;
	rsoJoined: boolean | null = null;

	rsoEvents: RsoEvent[] = [];

	universityName: string | null = null;

	private joinedRsos: Rso[] = [];
	private ownedRsos: Rso[] = [];

	ngOnInit() {
		this.rsoService.joinedRso$.subscribe((joinedRsos) => {
			this.joinedRsos = joinedRsos;
		});

		this.rsoService.ownedRso$.subscribe((ownedRsos) => {
			this.ownedRsos = ownedRsos;
		});

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

		// Subscribe to rso behaviorSubject
		this.modalService.rso$.subscribe((rso) => {
			this.rso = rso;

			if (rso != null) {
				this.rsoJoined = false;
				this.rsoOwned = false;

				// Get all RSO events
				this.rsoEventService
					.getAllRsoEventsByRsoId(rso.rsoid)
					.subscribe((rsoEvents) => {
						sortEventByDate(rsoEvents);
						this.rsoEvents = rsoEvents;
					});

				// Get if user joined RSO
				for (let i = 0; i < this.joinedRsos.length; i++) {
					if (this.joinedRsos[i].rsoid === rso.rsoid) {
						this.rsoJoined = true;
						break;
					}
				}

				// Get if user owns RSO
				for (let i = 0; i < this.ownedRsos.length; i++) {
					if (this.ownedRsos[i].rsoid === rso.rsoid) {
						this.rsoOwned = true;
						break;
					}
				}
			}
		});
	}

	onClose() {
		this.modalService.setEvent(null);
		this.modalService.setRso(null);
	}
}
