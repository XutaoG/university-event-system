import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
	apiSignUpRoute,
	apiLoginRoute,
	apiGetUserRoute,
} from '../../constants/api-routes';
import { SignUpForm, LoginForm, User } from '../../types/auth-types';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	http = inject(HttpClient);

	public signUp(signUpReq: SignUpForm) {
		const url = environment.apiUrl + apiSignUpRoute;
		return this.http.post<void>(url, signUpReq, {
			observe: 'response',
			withCredentials: true,
		});
	}

	public login(loginReq: LoginForm) {
		const url = environment.apiUrl + apiLoginRoute;
		return this.http.post<void>(url, loginReq, {
			observe: 'response',
			withCredentials: true,
		});
	}

	public getUser() {
		const url = environment.apiUrl + apiGetUserRoute;
		return this.http.get<User>(url, {
			observe: 'response',
			withCredentials: true,
		});
	}
}
