import { Component, inject, input } from '@angular/core';
import { Rso } from '../../../types/rso-types';
import { CardContainerComponent } from '../../card-container/card-container.component';
import { ModalService } from '../../../services/modal/modal.service';

@Component({
	selector: 'app-rso-container',
	imports: [CardContainerComponent],
	templateUrl: './rso-container.component.html',
	styleUrl: './rso-container.component.scss',
})
export class RsoContainerComponent {
	private modalService = inject(ModalService);
	rso = input.required<Rso>();

	onViewDetail() {
		this.modalService.setRso(this.rso());
	}
}
