import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
	selector: 'app-login',
	imports: [RouterLink],
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss', '../auth-styles.scss'],
})
export class LoginComponent {}
