export interface Rso {
	rsoid: number;
	name: string;
	description: string;
	universityID: number;
	adminID: number;
	active: boolean;
}

export interface AddRsoForm {
	name: string;
	description: string;
	memberEmails: string[];
}
