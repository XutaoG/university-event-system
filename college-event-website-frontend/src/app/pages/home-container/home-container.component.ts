import { Component, inject, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { NavComponent } from '../../components/nav/nav.component';
import { ModalComponent } from '../../components/modal/modal.component';
import { RsoService } from '../../services/rso/rso.service';

@Component({
	selector: 'app-home-container',
	imports: [MatIconModule, NavComponent, RouterOutlet, ModalComponent],
	templateUrl: './home-container.component.html',
	styleUrl: './home-container.component.scss',
})
export class HomeContainerComponent implements OnInit {
	private authService = inject(AuthService);
	private router = inject(Router);

	private rsoService = inject(RsoService);

	logout() {
		this.authService.logout().subscribe(() => {
			this.router.navigateByUrl('/login');
		});
	}

	ngOnInit() {
		this.authService.getUser().subscribe();

		this.rsoService.rsoFresh();
	}
}
