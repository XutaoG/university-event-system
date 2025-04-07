import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { apiGetRsoEventsAllRoute } from '../../../constants/api-routes';
import urlJoin from 'url-join';
import { RsoEvent } from '../../../types/event-types';

@Injectable({
	providedIn: 'root',
})
export class RsoEventsService {
	private http = inject(HttpClient);

	getAllRsoEvents() {
		const url = urlJoin(environment.apiUrl, apiGetRsoEventsAllRoute);

		return this.http.get<RsoEvent[]>(url, { withCredentials: true });
	}
}
