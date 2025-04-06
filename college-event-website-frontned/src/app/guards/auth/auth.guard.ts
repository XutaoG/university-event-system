import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { catchError, EMPTY, map, of, switchMap } from 'rxjs';
import { User } from '../../types/auth-types';

export const authGuard: CanActivateFn = (route, state) => {
	const authService = inject(AuthService);
	const router = inject(Router);

	return authService.user$.pipe(
		switchMap((user) => {
			if (user != null) {
				return of(true);
			}
			return authService.getUser();
		}),
		map((val) => {
			// If val is a boolean, it can only be true
			if (typeof val === 'boolean') {
				return val;
			}

			// Val is a user, is not null, return true;
			if (val != null) {
				return true;
			}

			return false;
		}),
		catchError(() => {
			router.navigateByUrl('/login');
			return EMPTY;
		})
	);
};
