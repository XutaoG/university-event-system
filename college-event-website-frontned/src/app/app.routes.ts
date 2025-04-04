import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: '',
		pathMatch: 'full',
		redirectTo: '/home',
	},
	// Authentication page
	{
		path: '',
		title: 'auth',
		loadComponent: async () => {
			const m = await import(
				'./components/auth/auth-layout/auth-layout.component'
			);
			return m.AuthLayoutComponent;
		},
		children: [
			{
				// "/login" page
				path: 'login',
				pathMatch: 'full',
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
		path: 'home',
		pathMatch: 'full',
		loadComponent: async () => {
			const m = await import('./components/home/home.component');
			return m.HomeComponent;
		},
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
