import { Component, inject, OnInit } from '@angular/core';
import { GridContainerComponent } from '../../components/grid-container/grid-container.component';
import { RsoService } from '../../services/rso/rso.service';
import { Rso } from '../../types/rso-types';
import { RsoContainerComponent } from '../../components/rso/rso-container/rso-container.component';
import { AddRsoComponent } from '../../components/rso/add-rso/add-rso.component';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-rso',
	imports: [
		GridContainerComponent,
		RsoContainerComponent,
		AddRsoComponent,
		CommonModule,
	],
	templateUrl: './rso.component.html',
	styleUrl: './rso.component.scss',
})
export class RsoComponent implements OnInit {
	private rsoService = inject(RsoService);
	private authService = inject(AuthService);

	user$ = this.authService.user$;
	ownedRsos: Rso[] = [];
	joinedRsos: Rso[] = [];
	availableRsos: Rso[] = [];

	ngOnInit() {
		this.rsoService.ownedRso$.subscribe((rsos) => {
			this.ownedRsos = rsos;
		});

		this.rsoService.joinedRso$.subscribe((rsos) => {
			this.joinedRsos = rsos;
		});

		this.rsoService.availableRso$.subscribe((rsos) => {
			this.availableRsos = rsos;
		});
	}
}
