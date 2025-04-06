import { Component, inject } from '@angular/core';
import { UniversityService } from '../../services/university/university.service';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'app-university-info',
	imports: [CommonModule, MatIconModule],
	templateUrl: './university-info.component.html',
	styleUrl: './university-info.component.scss',
})
export class UniversityInfoComponent {
	private uniService = inject(UniversityService);
	university$ = this.uniService.university$;
}
