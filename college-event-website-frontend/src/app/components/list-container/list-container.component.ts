import { Component, input } from '@angular/core';

@Component({
	selector: 'app-list-container',
	imports: [],
	templateUrl: './list-container.component.html',
	styleUrl: './list-container.component.scss',
})
export class ListContainerComponent {
	containerTitle = input.required<string>();
}
