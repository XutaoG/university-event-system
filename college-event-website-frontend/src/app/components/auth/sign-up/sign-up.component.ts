import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
	selector: 'app-sign-up',
	imports: [RouterLink],
	templateUrl: './sign-up.component.html',
	styleUrls: ['./sign-up.component.scss', '../auth-styles.scss'],
})
export class SignUpComponent {}
