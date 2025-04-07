import { Component, input } from '@angular/core';
import { Rso } from '../../../types/rso-types';
import { CardContainerComponent } from '../../card-container/card-container.component';

@Component({
	selector: 'app-rso-container',
	imports: [CardContainerComponent],
	templateUrl: './rso-container.component.html',
	styleUrl: './rso-container.component.scss',
})
export class RsoContainerComponent {
	rso = input.required<Rso>();
}
