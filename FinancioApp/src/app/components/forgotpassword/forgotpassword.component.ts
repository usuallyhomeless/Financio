import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApicallService } from 'src/app/apicall.service';
import { JsonResponse } from 'src/app/Models/common';
import { ResetPassword } from 'src/app/Models/resetPassword';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css'],
})
export class ForgotpasswordComponent implements OnInit {
  forgotPasswordForm = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.email,
    ]),
  });

  resetPasswordForm = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.email,
    ]),
    token: new FormControl('', [Validators.required]),
    password: new FormControl('', [
      Validators.required,
      Validators.maxLength(30),
      Validators.pattern(
        '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$'
      ),
    ]),
  });

  serverJsonResponse?: JsonResponse;

  constructor(private service: ApicallService, private router: Router) {}

  ngOnInit(): void {}

  ForgotPasswordSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      this.service
        .postForgotPassword(this.forgotPasswordForm.value.email)
        .subscribe((response) => {
          this.serverJsonResponse = response;
        });
    }
  }

  ResetPasswordSubmit(): void {
    if (this.resetPasswordForm.valid) {
      var resetPassword: ResetPassword = new ResetPassword();
      resetPassword.email = this.resetPasswordForm.value.email;
      resetPassword.password = this.resetPasswordForm.value.password;
      resetPassword.token = this.resetPasswordForm.value.token;
      this.service.postResetPassword(resetPassword).subscribe((response) => {
        this.serverJsonResponse = response;
        if (response.status == 200) {
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        }
      });
    }
  }

  closeAlert() {
    this.serverJsonResponse = undefined;
  }
}
