import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
	const authService = inject(AuthService);
	const router = inject(Router);

	return authService.getUser().pipe(
		map((user) => {
			if (user != null) {
				// User is authenticated
				return true;
			} else {
				// User is not authenticated -> redirect to /login
				router.navigate(['/login']);
				return false;
			}
		}),
		catchError(() => {
			// Error, redirect to /login
			router.navigate(['/login']);
			return of(false);
		})
	);
};
