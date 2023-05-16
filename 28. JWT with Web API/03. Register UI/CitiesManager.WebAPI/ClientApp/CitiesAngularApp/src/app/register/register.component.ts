import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterUser } from '../models/register-user';
import { AccountService } from '../services/account.service';
import { CompareValidation } from '../validators/custom-validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  isRegisterFormSubmitted: boolean = false;


  constructor(private accountService: AccountService, private router: Router) {
    this.registerForm = new FormGroup({
      personName: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.email]),
      phoneNumber: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      confirmPassword: new FormControl(null, [Validators.required])
    },
      {
        validators: [CompareValidation("password", "confirmPassword")]
      });
  }


  get register_personNameControl(): any {
    return this.registerForm.controls["personName"];
  }

  get register_emailControl(): any {
    return this.registerForm.controls["email"];
  }

  get register_phoneNumberControl(): any {
    return this.registerForm.controls["phoneNumber"];
  }

  get register_passwordControl(): any {
    return this.registerForm.controls["password"];
  }

  get register_confirmPasswordControl(): any {
    return this.registerForm.controls["confirmPassword"];
  }

  registerSubmitted() {
    this.isRegisterFormSubmitted = true;

    if (this.registerForm.valid) {

      this.accountService.postRegister(this.registerForm.value).subscribe({
        next: (response: RegisterUser) => {
          console.log(response);

          this.isRegisterFormSubmitted = false;

          this.router.navigate(['/cities']);

          this.registerForm.reset();
        },

        error: (error) => {
          console.log(error);
        },

        complete: () => { },
      });
    }
  }
}
