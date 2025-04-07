import { Component, input } from '@angular/core';

@Component({
	selector: 'app-grid-container',
	imports: [],
	templateUrl: './grid-container.component.html',
	styleUrl: './grid-container.component.scss',
})
export class GridContainerComponent {
	containerTitle = input.required<string>();
}
