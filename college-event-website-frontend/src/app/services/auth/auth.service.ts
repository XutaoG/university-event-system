import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
	apiSignUpRoute,
	apiLoginRoute,
	apiGetUserRoute,
	apiLogoutRoute,
} from '../../constants/api-routes';
import { SignUpForm, LoginForm, User } from '../../types/auth-types';
import {
	BehaviorSubject,
	catchError,
	EMPTY,
	NotFoundError,
	of,
	tap,
	throwError,
} from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	private http = inject(HttpClient);
	private userSubject = new BehaviorSubject<User | null>(null);

	user$ = this.userSubject.asObservable();

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
		return this.http
			.get<User>(url, {
				withCredentials: true,
			})
			.pipe(
				tap((user) => {
					this.userSubject.next(user);
				})
			);
	}

	public logout() {
		const url = environment.apiUrl + apiLogoutRoute;
		return this.http.post<void>(url, null, { withCredentials: true }).pipe(
			tap(() => {
				this.userSubject.next(null);
			})
		);
	}
}
