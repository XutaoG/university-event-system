import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from '../nav/nav.component';

@Component({
	selector: 'app-home-container',
	imports: [MatIconModule, NavComponent, RouterOutlet],
	templateUrl: './home-container.component.html',
	styleUrl: './home-container.component.scss',
})
export class HomeContainerComponent {}
