import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { catchError, map, of } from 'rxjs';

export const redirectHomeGuard: CanActivateChildFn = (route, state) => {
	const router = inject(Router);
	const authService = inject(AuthService);

	return authService.getUser().pipe(
		map((user) => {
			if (user == null) {
				// Use is not authenticated
				return true;
			} else {
				// User is authenticated -> redirect to /home
				router.navigate(['/home']);
				return false;
			}
		}),
		catchError(() => {
			// Error, allow access to /login
			return of(true);
		})
	);
};
