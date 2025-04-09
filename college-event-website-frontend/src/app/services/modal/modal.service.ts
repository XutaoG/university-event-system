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

	private isAddingEventSubject = new BehaviorSubject(false);
	private isAddingRsoSubject = new BehaviorSubject(false);

	event$ = this.eventSubject.asObservable();
	rso$ = this.rsoSubject.asObservable();

	isAddingEvent$ = this.isAddingEventSubject.asObservable();
	isAddingRso$ = this.isAddingRsoSubject.asObservable();

	setEvent(event: EventT | null) {
		this.rsoSubject.next(null);
		this.eventSubject.next(event);
		this.isAddingEventSubject.next(false);
		this.isAddingRsoSubject.next(false);
	}

	setRso(rso: Rso | null) {
		this.eventSubject.next(null);
		this.rsoSubject.next(rso);
		this.isAddingEventSubject.next(false);
		this.isAddingRsoSubject.next(false);
	}

	setAddingRso(isAddingRso: boolean) {
		this.isAddingRsoSubject.next(isAddingRso);
		this.isAddingEventSubject.next(false);
		this.eventSubject.next(null);
		this.rsoSubject.next(null);
	}

	setAddingEvent(isAddingEvent: boolean) {
		this.isAddingRsoSubject.next(false);
		this.isAddingEventSubject.next(isAddingEvent);
		this.eventSubject.next(null);
		this.rsoSubject.next(null);
	}
}
