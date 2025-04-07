import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { University } from '../../types/university-types';
import { environment } from '../../../environments/environment';
import { apiGetUniversityRoute } from '../../constants/api-routes';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})
export class UniversityService {
	private http = inject(HttpClient);
	private universitySubject = new BehaviorSubject<University | null>(null);
	university$ = this.universitySubject.asObservable();

	getUniversity() {
		const url = environment.apiUrl + apiGetUniversityRoute;
		return this.http.get<University>(url, { withCredentials: true }).pipe(
			tap((uni) => {
				this.universitySubject.next(uni);
			})
		);
	}
}
