import { Component, inject, OnInit } from '@angular/core';
import { RsoEventsService } from '../../../services/events/rso-events/rso-events.service';
import { ModalService } from '../../../services/modal/modal.service';
import { RsoService } from '../../../services/rso/rso.service';
import { Rso } from '../../../types/rso-types';
import { RsoEvent } from '../../../types/event-types';
import { sortEventByDate } from '../../../utils';
import { ListContainerComponent } from '../../list-container/list-container.component';
import { CommonModule } from '@angular/common';
import { EventEntryContainerComponent } from '../../events/event-entry-container/event-entry-container.component';

@Component({
	selector: 'app-rso-modal-view',
	imports: [
		ListContainerComponent,
		CommonModule,
		EventEntryContainerComponent,
	],
	templateUrl: './rso-modal-view.component.html',
	styleUrls: ['./rso-modal-view.component.scss', '../modal.component.scss'],
})
export class RsoModalViewComponent implements OnInit {
	private modalService = inject(ModalService);
	private rsoService = inject(RsoService);
	private rsoEventService = inject(RsoEventsService);

	rso: Rso | null = null;

	rsoOwned: boolean | null = null;
	rsoJoined: boolean | null = null;

	rsoEvents: RsoEvent[] = [];

	private joinedRsos: Rso[] = [];
	private ownedRsos: Rso[] = [];

	ngOnInit() {
		this.rsoService.joinedRso$.subscribe((joinedRsos) => {
			this.joinedRsos = joinedRsos;
		});

		this.rsoService.ownedRso$.subscribe((ownedRsos) => {
			this.ownedRsos = ownedRsos;
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

	joinRso() {
		this.rsoService.joinRso(this.rso!.rsoid).subscribe(() => {
			this.onClose();
			this.rsoEventService.getAllRsoEvents().subscribe();
		});
	}

	leaveRso() {
		this.rsoService.leaveRso(this.rso!.rsoid).subscribe(() => {
			this.onClose();
			this.rsoEventService.getAllRsoEvents().subscribe();
		});
	}

	onModalOpen() {
		this.modalService.currentRsoId = this.rso!.rsoid;
		this.modalService.setAddingEvent(true);
	}
}
