import { Routes } from '@angular/router';
import { authGuard } from './guards/auth/auth.guard';
import { redirectHomeGuard } from './guards/auth/redirect-home.guard';

export const routes: Routes = [
	{
		path: '',
		pathMatch: 'full',
		redirectTo: '/home',
	},
	{
		path: '',
		pathMatch: 'prefix',
		loadComponent: async () => {
			const m = await import('./pages/auth/auth.component');
			return m.AuthComponent;
		},
		canActivate: [redirectHomeGuard],
		children: [
			{
				// "/login" page
				path: 'login',
				pathMatch: 'full',
				title: 'Login',
				loadComponent: async () => {
					const m = await import(
						'./components/auth/login/login.component'
					);
					return m.LoginComponent;
				},
			},
			{
				// "/sign-up" page
				path: 'sign-up',
				pathMatch: 'full',
				title: 'Sign Up',
				loadComponent: async () => {
					const m = await import(
						'./components/auth/sign-up/sign-up.component'
					);
					return m.SignUpComponent;
				},
			},
		],
	},
	// Home page
	{
		path: '',
		pathMatch: 'prefix',
		loadComponent: async () => {
			const m = await import(
				'./components/home-container/home-container.component'
			);
			return m.HomeContainerComponent;
		},
		canActivate: [authGuard],
		children: [
			{
				path: 'home',
				pathMatch: 'prefix',
				title: 'Home',
				loadComponent: async () => {
					const m = await import('./pages/home/home.component');
					return m.HomeComponent;
				},
			},
		],
	},
	{
		path: '**',
		loadComponent: async () => {
			const m = await import(
				'./components/not-found/not-found.component'
			);
			return m.NotFoundComponent;
		},
	},
];
