import { AfterViewInit, Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import {
	FormBuilder,
	FormControl,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { SignUpForm } from '../../../types/auth-types';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'app-sign-up',
	imports: [RouterLink, ReactiveFormsModule, MatIconModule],
	templateUrl: './sign-up.component.html',
	styleUrls: ['./sign-up.component.scss', '../auth-styles.scss'],
})
export class SignUpComponent implements AfterViewInit {
	private authService = inject(AuthService);
	private router = inject(Router);
	private formBuilder = inject(FormBuilder);

	isFetching = false;

	signUpForm = this.formBuilder.group({
		name: new FormControl<string>('', {
			nonNullable: true,
			validators: [Validators.required],
		}),
		email: new FormControl<string>('', {
			nonNullable: true,
			validators: [Validators.required, Validators.email],
		}),
		password: new FormControl<string>('', {
			nonNullable: true,
			validators: [Validators.required],
		}),
		userRole: new FormControl<'Student' | 'Admin' | 'SuperAdmin'>(
			'Student',
			{
				nonNullable: true,
				validators: [Validators.required],
			}
		),
	});

	ngAfterViewInit() {
		this.signUpForm.controls['userRole'].setValue('Student');
	}

	submit() {
		if (this.signUpForm.valid) {
			this.isFetching = true;

			// Assemble form data
			const formData: SignUpForm = {
				name: this.signUpForm.value.name!,
				email: this.signUpForm.value.email!,
				password: this.signUpForm.value.password!,
				userRole: this.signUpForm.value.userRole!,
			};

			this.authService.signUp(formData).subscribe(() => {
				this.isFetching = false;
				this.router.navigateByUrl('/login');
			});
		}
	}
}
