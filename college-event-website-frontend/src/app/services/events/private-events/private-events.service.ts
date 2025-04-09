import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import urlJoin from 'url-join';
import { environment } from '../../../../environments/environment';
import {
	apiAddPrivateEventRoute,
	apiGetPrivateEventsRoute,
} from '../../../constants/api-routes';
import { AddEventForm, PrivateEvent } from '../../../types/event-types';

@Injectable({
	providedIn: 'root',
})
export class PrivateEventsService {
	private http = inject(HttpClient);

	getAllPrivateEvents() {
		const url = urlJoin(environment.apiUrl, apiGetPrivateEventsRoute);

		return this.http.get<PrivateEvent[]>(url, { withCredentials: true });
	}

	addPrivateEvent(addEventForm: AddEventForm) {
		const url = urlJoin(environment.apiUrl, apiAddPrivateEventRoute);

		return this.http.post<PrivateEvent>(url, addEventForm, {
			withCredentials: true,
		});
	}
}
