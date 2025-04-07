import { EventT } from '../types/event-types';

export const sortEventByDate = <T extends EventT>(events: T[]): T[] => {
	events.sort((e1, e2) => {
		return (
			new Date(e1.eventDate).getTime() - new Date(e2.eventDate).getTime()
		);
	});

	return events;
};
