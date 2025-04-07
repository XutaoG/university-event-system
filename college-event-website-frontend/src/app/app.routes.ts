import { Routes } from '@angular/router';
import { authGuard } from './guards/auth/auth.guard';
import { redirectHomeGuard } from './guards/auth/redirect-home.guard';
import { RsoEventsComponent } from './components/events/rso-events/rso-events.component';
import { PublicEventsComponent } from './components/events/public-events/public-events.component';
import { PrivateEventsComponent } from './components/events/private-events/private-events.component';

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
		loadComponent: async () => {
			const m = await import(
				'./pages/home-container/home-container.component'
			);
			return m.HomeContainerComponent;
		},
		canActivate: [authGuard],
		children: [
			{
				path: 'home',
				title: 'Home',
				loadComponent: async () => {
					const m = await import('./pages/home/home.component');
					return m.HomeComponent;
				},
			},
			{
				path: 'event',
				title: 'Events',
				loadComponent: async () => {
					const m = await import('./pages/event/event.component');
					return m.EventComponent;
				},
				children: [
					{
						path: '',
						pathMatch: 'full',
						redirectTo: 'rso',
					},
					{
						path: 'rso',
						title: 'RSO Events',
						component: RsoEventsComponent,
					},
					{
						path: 'public',
						title: 'Public Events',
						component: PublicEventsComponent,
					},
					{
						path: 'private',
						title: 'Private Events',
						component: PrivateEventsComponent,
					},
				],
			},
			{
				path: 'rso',
				title: 'RSOs',
				loadComponent: async () => {
					const m = await import('./pages/rso/rso.component');
					return m.RsoComponent;
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
