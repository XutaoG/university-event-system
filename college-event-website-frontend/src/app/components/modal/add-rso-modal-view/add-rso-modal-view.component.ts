import { Component, inject } from '@angular/core';
import { ModalService } from '../../../services/modal/modal.service';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { AddRsoForm } from '../../../types/rso-types';
import { RsoService } from '../../../services/rso/rso.service';

@Component({
	selector: 'app-add-rso-modal-view',
	imports: [ReactiveFormsModule],
	templateUrl: './add-rso-modal-view.component.html',
	styleUrls: [
		'./add-rso-modal-view.component.scss',
		'../modal.component.scss',
	],
})
export class AddRsoModalViewComponent {
	private modalService = inject(ModalService);
	private rsoService = inject(RsoService);
	private formBuilder = inject(FormBuilder);

	isFetching = false;

	addRsoForm = this.formBuilder.group({
		name: new FormControl<string>('', {
			nonNullable: true,
		}),
		description: new FormControl<string>('', {
			nonNullable: true,
		}),
		memberEmails: new FormControl<string>(''),
	});

	onCancel() {
		this.modalService.setAddingRso(false);
	}

	onAddRso() {
		if (this.addRsoForm.valid) {
			this.isFetching = true;

			// Assemble form data
			const formData: AddRsoForm = {
				name: this.addRsoForm.value.name!,
				description: this.addRsoForm.value.description!,
				memberEmails: this.addRsoForm.value
					.memberEmails!.trim()
					.split(',')
					.map((email) => {
						return email.trim();
					}),
			};

			this.rsoService.addRso(formData).subscribe(() => {
				this.isFetching = false;
				this.onCancel();
				this.rsoService.getOwnedRso().subscribe();
			});
		}
	}
}
