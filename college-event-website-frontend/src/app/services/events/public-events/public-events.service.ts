import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import urlJoin from 'url-join';
import { environment } from '../../../../environments/environment';
import { apiGetPublicEventsRoute } from '../../../constants/api-routes';
import { AddEventForm, PublicEvent } from '../../../types/event-types';

@Injectable({
	providedIn: 'root',
})
export class PublicEventsService {
	private http = inject(HttpClient);

	getAllPublicEvents() {
		const url = urlJoin(environment.apiUrl, apiGetPublicEventsRoute);

		return this.http.get<PublicEvent[]>(url, { withCredentials: true });
	}

	addPublicEvent(addEventForm: AddEventForm) {
		const url = urlJoin(environment.apiUrl, apiGetPublicEventsRoute);

		return this.http.post<PublicEvent>(url, addEventForm, {
			withCredentials: true,
		});
	}
}
