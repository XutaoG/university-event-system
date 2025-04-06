import { Component, input } from '@angular/core';

@Component({
	selector: 'app-vertical-container',
	imports: [],
	templateUrl: './vertical-container.component.html',
	styleUrl: './vertical-container.component.scss',
})
export class VerticalContainerComponent {
	containerTitle = input.required<string>();
}
