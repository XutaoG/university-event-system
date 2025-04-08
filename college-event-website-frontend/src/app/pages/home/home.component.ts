import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { UniversityService } from '../../services/university/university.service';
import { roleMap } from '../../types/auth-types';
import { MatIconModule } from '@angular/material/icon';
import { RsoService } from '../../services/rso/rso.service';
import { RsoJoinedComponent } from '../../components/rso/rso-joined/rso-joined.component';
import { UniversityInfoComponent } from '../../components/university-info/university-info.component';
import { UpcomingRsoEventsComponent } from '../../components/events/upcoming-rso-events/upcoming-rso-events.component';
import { ModalComponent } from '../../components/modal/modal.component';

@Component({
	selector: 'app-home',
	imports: [
		CommonModule,
		MatIconModule,
		RsoJoinedComponent,
		UpcomingRsoEventsComponent,
		UniversityInfoComponent,
	],
	templateUrl: './home.component.html',
	styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
	roleMap = roleMap;

	private authService = inject(AuthService);
	user$ = this.authService.user$;

	private uniService = inject(UniversityService);
	university$ = this.uniService.university$;

	private rsoService = inject(RsoService);

	ngOnInit() {
		this.uniService.getUniversity().subscribe();
	}
}
