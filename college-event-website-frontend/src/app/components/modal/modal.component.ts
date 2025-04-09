import { Component, inject, OnInit } from '@angular/core';
import { Rso } from '../../types/rso-types';
import { ModalService } from '../../services/modal/modal.service';
import { EventT } from '../../types/event-types';
import { CommonModule } from '@angular/common';
import { EventModalViewComponent } from './event-modal-view/event-modal-view.component';
import { RsoModalViewComponent } from './rso-modal-view/rso-modal-view.component';
import { AddRsoModalViewComponent } from './add-rso-modal-view/add-rso-modal-view.component';

@Component({
	selector: 'app-modal',
	imports: [
		CommonModule,
		EventModalViewComponent,
		RsoModalViewComponent,
		AddRsoModalViewComponent,
	],
	templateUrl: './modal.component.html',
	styleUrl: './modal.component.scss',
})
export class ModalComponent implements OnInit {
	private modalService = inject(ModalService);

	event: EventT | null = null;
	rso: Rso | null = null;

	isAddingRso: boolean = false;
	isAddingEvent: boolean = false;

	ngOnInit() {
		// Subscribe to event behaviorSubject
		this.modalService.event$.subscribe((event) => {
			this.event = event;
		});

		this.modalService.rso$.subscribe((rso) => {
			this.rso = rso;
		});

		this.modalService.isAddingRso$.subscribe((isAddingRso) => {
			this.isAddingRso = isAddingRso;
		});

		this.modalService.isAddingEvent$.subscribe((isAddEvent) => {
			this.isAddingEvent = isAddEvent;
		});
	}
}
