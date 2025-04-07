import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import urlJoin from 'url-join';
import { environment } from '../../../environments/environment';
import { apiGetRsoJoined } from '../../constants/api-routes';
import { Rso } from '../../types/rso-types';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class RsoService {
	private http = inject(HttpClient);

	private joinedRsoSubject = new BehaviorSubject<Rso[]>([]);
	joinedRso$ = this.joinedRsoSubject.asObservable();

	getJoinedRso() {
		const url = urlJoin(environment.apiUrl, apiGetRsoJoined);

		return this.http.get<Rso[]>(url, { withCredentials: true }).pipe(
			tap((rsos) => {
				this.joinedRsoSubject.next(rsos);
			})
		);
	}
}
