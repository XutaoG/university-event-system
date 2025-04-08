import { FormControl } from '@angular/forms';

export interface SignUpForm {
	name: string;
	email: string;
	password: string;
	userRole: 'Student' | 'Admin' | 'SuperAdmin';
}

export interface LoginForm {
	email: string;
	password: string;
}

export interface User {
	uid: number;
	name: string;
	email: string;
	userRole: 'Student' | 'Admin' | 'SuperAdmin';
	universityID: number;
}

export const roleMap: { Student: string; Admin: string; SuperAdmin: string } = {
	Student: 'Student',
	Admin: 'Admin',
	SuperAdmin: 'SuperAdmin',
};
