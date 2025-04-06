import { Component, inject, signal } from '@angular/core';
import {
	FormBuilder,
	FormControl,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginForm } from '../../../types/auth-types';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpStatusCode } from '@angular/common/http';
import { EMPTY, switchMap, tap } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'app-login',
	imports: [RouterLink, ReactiveFormsModule, CommonModule, MatIconModule],
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss', '../auth-styles.scss'],
})
export class LoginComponent {
	authService = inject(AuthService);
	router = inject(Router);
	formBuilder = inject(FormBuilder);

	isFetching = signal(false);

	constructor() {}

	loginForm = this.formBuilder.group({
		email: new FormControl<string>('', {
			nonNullable: true,
			validators: [Validators.required, Validators.email],
		}),
		password: new FormControl<string>('', {
			nonNullable: true,
			validators: [Validators.required],
		}),
	});

	submit() {
		if (this.loginForm.valid) {
			this.isFetching.set(true);

			// Assemble form data
			const formData: LoginForm = {
				email: this.loginForm.value.email!,
				password: this.loginForm.value.password!,
			};

			this.authService
				.login(formData)
				.pipe(
					switchMap((response) => {
						if (response.status == HttpStatusCode.Ok) {
							return this.authService.getUser();
						}
						return EMPTY;
					})
				)
				.subscribe((user) => {
					if (user != null) {
						this.router.navigateByUrl('/home');
					}
					this.isFetching.set(false);
				});
		}
	}
}
