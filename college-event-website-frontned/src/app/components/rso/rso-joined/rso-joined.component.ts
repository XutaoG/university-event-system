import { Component, inject, OnInit } from '@angular/core';
import { RsoService } from '../../../services/rso/rso.service';
import { Rso } from '../../../types/rso-types';
import { VerticalContainerComponent } from '../../vertical-container/vertical-container.component';
import { CommonModule } from '@angular/common';
import { RsoContainerComponent } from '../rso-container/rso-container.component';

@Component({
	selector: 'app-rso-joined',
	imports: [VerticalContainerComponent, CommonModule, RsoContainerComponent],
	templateUrl: './rso-joined.component.html',
	styleUrl: './rso-joined.component.scss',
})
export class RsoJoinedComponent implements OnInit {
	rsoService = inject(RsoService);
	joinedRsos: Rso[] = [];

	ngOnInit() {
		this.rsoService.joinedRso$.subscribe((rsos) => {
			this.joinedRsos = rsos;
		});
	}
}
