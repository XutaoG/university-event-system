import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { EventNavComponent } from '../../components/event-nav/event-nav.component';

@Component({
	selector: 'app-event',
	imports: [RouterOutlet, EventNavComponent],
	templateUrl: './event.component.html',
	styleUrl: './event.component.scss',
})
export class EventComponent {}
