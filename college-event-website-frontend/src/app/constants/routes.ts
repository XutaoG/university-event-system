import { Route } from '../types';

export const routes: Route[] = [
	{
		header: 'Home',
		routeTo: '/home',
		icon: 'home',
	},
	{
		header: 'Events',
		routeTo: '/event',
		icon: 'event',
	},
	{
		header: 'RSOs',
		routeTo: '/rso',
		icon: 'group',
	},
];

export const eventRoutes: Route[] = [
	{
		header: 'RSO Events',
		routeTo: '/event/rso',
		icon: 'group',
	},
	{
		header: 'Private Events',
		routeTo: '/event/private',
		icon: 'school',
	},

	{
		header: 'Public Events',
		routeTo: '/event/public',
		icon: 'public',
	},
];
