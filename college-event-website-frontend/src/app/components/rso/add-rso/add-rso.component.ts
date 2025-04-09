import { Component, inject } from '@angular/core';
import { ModalService } from '../../../services/modal/modal.service';

@Component({
	selector: 'app-add-rso',
	imports: [],
	templateUrl: './add-rso.component.html',
	styleUrl: './add-rso.component.scss',
})
export class AddRsoComponent {
	private modalService = inject(ModalService);

	onRsoAdd() {
		this.modalService.setAddingRso(true);
	}
}
