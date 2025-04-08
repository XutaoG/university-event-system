import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { EventT } from '../../types/event-types';
import { Rso } from '../../types/rso-types';

@Injectable({
	providedIn: 'root',
})
export class ModalService {
	private eventSubject = new BehaviorSubject<EventT | null>(null);
	private rsoSubject = new BehaviorSubject<Rso | null>(null);

	event$ = this.eventSubject.asObservable();
	rso$ = this.rsoSubject.asObservable();

	setEvent(event: EventT | null) {
		this.eventSubject.next(event);
	}

	setRso(rso: Rso | null) {
		this.rsoSubject.next(rso);
	}
}
