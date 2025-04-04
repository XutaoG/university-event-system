import { Component, inject } from '@angular/core';
import {
	FormBuilder,
	FormControl,
	FormGroup,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginForm } from '../../../types/auth-types';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpStatusCode } from '@angular/common/http';

@Component({
	selector: 'app-login',
	imports: [RouterLink, ReactiveFormsModule, CommonModule],
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss', '../auth-styles.scss'],
})
export class LoginComponent {
	authService = inject(AuthService);
	router = inject(Router);
	formBuilder = inject(FormBuilder);

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
			// Assemble form data
			const formData: LoginForm = {
				email: this.loginForm.value.email!,
				password: this.loginForm.value.password!,
			};

			this.authService.login(formData).subscribe((response) => {
				if (response.status == HttpStatusCode.Ok) {
					this.router.navigateByUrl('/home');
				}
			});
		}
	}
}
