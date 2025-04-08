import { Component, inject, OnInit } from '@angular/core';
import { GridContainerComponent } from '../../components/grid-container/grid-container.component';
import { RsoService } from '../../services/rso/rso.service';
import { Rso } from '../../types/rso-types';
import { RsoContainerComponent } from '../../components/rso/rso-container/rso-container.component';

@Component({
	selector: 'app-rso',
	imports: [GridContainerComponent, RsoContainerComponent],
	templateUrl: './rso.component.html',
	styleUrl: './rso.component.scss',
})
export class RsoComponent implements OnInit {
	private rsoService = inject(RsoService);

	joinedRsos: Rso[] = [];

	availableRsos: Rso[] = [];

	ngOnInit() {
		this.rsoService.joinedRso$.subscribe((rsos) => {
			this.joinedRsos = rsos;
		});

		this.rsoService.availableRso$.subscribe((rsos) => {
			this.availableRsos = rsos;
		});
	}
}
