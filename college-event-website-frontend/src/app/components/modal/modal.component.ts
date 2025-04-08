import { Component, inject, OnInit } from '@angular/core';
import { Rso } from '../../types/rso-types';
import { ModalService } from '../../services/modal/modal.service';
import { EventT } from '../../types/event-types';
import { CommonModule } from '@angular/common';
import { EventModalViewComponent } from './event-modal-view/event-modal-view.component';
import { RsoModalViewComponent } from './rso-modal-view/rso-modal-view.component';

@Component({
	selector: 'app-modal',
	imports: [CommonModule, EventModalViewComponent, RsoModalViewComponent],
	templateUrl: './modal.component.html',
	styleUrl: './modal.component.scss',
})
export class ModalComponent implements OnInit {
	private modalService = inject(ModalService);

	event: EventT | null = null;
	rso: Rso | null = null;

	ngOnInit() {
		// Subscribe to event behaviorSubject
		this.modalService.event$.subscribe((event) => {
			this.event = event;
		});

		this.modalService.rso$.subscribe((rso) => {
			this.rso = rso;
		});
	}
}
