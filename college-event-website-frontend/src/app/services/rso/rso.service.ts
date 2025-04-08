import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import urlJoin from 'url-join';
import { environment } from '../../../environments/environment';
import {
	apiGetAvailableRsoRoute,
	apiGetRsoByIdRoute,
	apiGetRsoJoinedRoute,
	apiGetRsoOwnedRoute,
	apiJoinRsoRoute,
	apiLeaveRsoRoute,
} from '../../constants/api-routes';
import { Rso } from '../../types/rso-types';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class RsoService {
	private http = inject(HttpClient);

	private joinedRsoSubject = new BehaviorSubject<Rso[]>([]);
	joinedRso$ = this.joinedRsoSubject.asObservable();

	private ownedRsoSubject = new BehaviorSubject<Rso[]>([]);
	ownedRso$ = this.ownedRsoSubject.asObservable();

	private availableRsoSubject = new BehaviorSubject<Rso[]>([]);
	availableRso$ = this.availableRsoSubject.asObservable();

	getRsoById(rsoId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiGetRsoByIdRoute,
			rsoId.toString()
		);

		return this.http.get<Rso>(url, { withCredentials: true });
	}

	getJoinedRso() {
		const url = urlJoin(environment.apiUrl, apiGetRsoJoinedRoute);

		return this.http.get<Rso[]>(url, { withCredentials: true }).pipe(
			tap((rsos) => {
				this.joinedRsoSubject.next(rsos);
			})
		);
	}

	getOwnedRso() {
		const url = urlJoin(environment.apiUrl, apiGetRsoOwnedRoute);

		return this.http.get<Rso[]>(url, { withCredentials: true }).pipe(
			tap((rsos) => {
				this.ownedRsoSubject.next(rsos);
			})
		);
	}

	getAvailableRso() {
		const url = urlJoin(environment.apiUrl, apiGetAvailableRsoRoute);

		return this.http.get<Rso[]>(url, { withCredentials: true }).pipe(
			tap((rsos) => {
				this.availableRsoSubject.next(rsos);
			})
		);
	}

	joinRso(rsoId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiJoinRsoRoute,
			rsoId.toString()
		);

		return this.http
			.post<void>(url, null, { withCredentials: true })
			.pipe(tap(() => this.rsoFresh()));
	}

	leaveRso(rsoId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiLeaveRsoRoute,
			rsoId.toString()
		);

		return this.http
			.post<void>(url, null, { withCredentials: true })
			.pipe(tap(() => this.rsoFresh()));
	}

	rsoFresh() {
		this.getAvailableRso().subscribe();
		this.getJoinedRso().subscribe();
		this.getOwnedRso().subscribe();
	}
}
