import { EventT } from '../types/event-types';

export const sortEventByDate = <T extends EventT>(events: T[]): T[] => {
	events.sort((e1, e2) => {
		return (
			new Date(e1.eventDate).getTime() - new Date(e2.eventDate).getTime()
		);
	});

	return events;
};

export const parseDate = (date: Date) => {
	const eventDate = new Date(date);
	return `${eventDate.getMonth() + 1}/${eventDate.getDate()}/${eventDate
		.getFullYear()
		.toString()
		.substring(2)}`;
};

export const parseTime = (time: string) => {
	return new Date('1970-01-01T' + time + 'Z').toLocaleString('en-US', {
		hour: 'numeric',
		minute: 'numeric',
		hour12: true,
	});
};
