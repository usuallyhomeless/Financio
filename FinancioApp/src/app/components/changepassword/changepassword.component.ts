import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from 'src/app/apicall.service';
import { JsonResponse } from 'src/app/Models/common';

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css'],
})
export class ChangepasswordComponent implements OnInit {
  serverJsonResponse?: JsonResponse;

  constructor(
    public service: ApicallService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    var routeString: string = '';
    this.route.snapshot.url.forEach((element) => {
      routeString = routeString + '/' + element.path;
    });

    this.service.isLoggedin().subscribe((response) => {
      if (!response) {
        this.router.navigate([`/login/${encodeURIComponent(routeString)}`]);
      }
    });
  }

  ngOnInit(): void {}

  changePasswordForm = new FormGroup({
    currentPassword: new FormControl('', [Validators.required]),
    newPassword: new FormControl('', [
      Validators.required,
      Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$'),
      Validators.maxLength(30),
    ]),
    confirmNewPassword: new FormControl('', [
      Validators.required,
      Validators.maxLength(30),
      this.pwdMatchValidator,
    ]),
  });

  pwdMatchValidator(frm: any) {
    return frm?.parent?.controls?.newPassword?.value ===
      frm?.parent?.controls?.confirmNewPassword?.value
      ? null
      : { mismatch: true };
  }

  OnSubmit(): void {
    if (this.changePasswordForm.valid) {
      this.service
        .postChangePassword(this.changePasswordForm.value)
        .subscribe((data: JsonResponse) => {
          this.serverJsonResponse = data;
          if (this.serverJsonResponse.status == 200) {
            setTimeout(() => {
              this.router.navigate(['']);
            }, 2000);
          }
        });
    }
  }

  closeAlert() {
    this.serverJsonResponse = undefined;
  }
}
