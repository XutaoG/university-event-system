import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import {
	apiAddRsoEventRoute,
	apiGetRsoEventsAllRoute,
	apiGetRsoEventsByRsoIdRoute,
} from '../../../constants/api-routes';
import urlJoin from 'url-join';
import { AddEventForm, RsoEvent } from '../../../types/event-types';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class RsoEventsService {
	private http = inject(HttpClient);

	private allRsoEvents = new BehaviorSubject<RsoEvent[] | null>(null);
	allRsoEvents$ = this.allRsoEvents.asObservable();

	getAllRsoEvents() {
		const url = urlJoin(environment.apiUrl, apiGetRsoEventsAllRoute);

		return this.http.get<RsoEvent[]>(url, { withCredentials: true }).pipe(
			tap((rsos) => {
				this.allRsoEvents.next(rsos);
			})
		);
	}

	getAllRsoEventsByRsoId(rsoId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiGetRsoEventsByRsoIdRoute,
			rsoId.toString()
		);

		return this.http.get<RsoEvent[]>(url, { withCredentials: true });
	}

	addRsoEvent(addEventForm: AddEventForm, rsoId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiAddRsoEventRoute,
			rsoId.toString()
		);

		return this.http.post<RsoEvent>(url, addEventForm, {
			withCredentials: true,
		});
	}
}
