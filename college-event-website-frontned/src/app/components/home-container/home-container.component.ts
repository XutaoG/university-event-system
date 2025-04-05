import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterOutlet } from '@angular/router';
import { NavComponent } from '../nav/nav.component';
import { AuthService } from '../../services/auth/auth.service';

@Component({
	selector: 'app-home-container',
	imports: [MatIconModule, NavComponent, RouterOutlet],
	templateUrl: './home-container.component.html',
	styleUrl: './home-container.component.scss',
})
export class HomeContainerComponent {
	authService = inject(AuthService);
	router = inject(Router);

	logout() {
		this.authService.logout().subscribe(() => {
			this.router.navigateByUrl('/login');
		});
	}
}
