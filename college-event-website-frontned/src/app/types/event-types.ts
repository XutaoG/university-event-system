export interface EventT {
	eventID: number;
	name: string;
	eventCategory: string;
	description: string | null;
	eventTime: string;
	eventTimeEnd: string;
	eventDate: Date;
	contactPhone: string;
	contactEmail: string;
	locID: number;
	location: Location;
	adminID: number;
	universityID: number;
}

export interface Location {
	locID: number;
	name: string;
	latitude: number;
	longitude: number;
	address: string;
}

export interface PublicEvent extends EventT {
	approved: boolean;
	universityID: number;
}

export interface PrivateEvent extends EventT {
	universityID: number;
}

export interface RsoEvent extends EventT {
	rsoid: number;
}
