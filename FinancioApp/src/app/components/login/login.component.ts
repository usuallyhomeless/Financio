import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from '../../apicall.service';
import { JsonResponse } from '../../Models/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  serverJsonResponse?: JsonResponse;

  loginForm = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
    ]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(
    private service: ApicallService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {}

  OnSubmit(): void {
    this.serverJsonResponse = undefined;
    if (this.loginForm.valid) {
      this.service.postlogin(this.loginForm.value).subscribe((response) => {
        this.serverJsonResponse = response;
        var nextPage = this.route.snapshot.paramMap.get('next?');
        if (this.serverJsonResponse.status == 200) {
          if (nextPage != null)
            this.router.navigate([decodeURIComponent(nextPage)]);
          else this.router.navigate(['']);
        }
      });
    }
  }

  closeAlert(): void {
    this.serverJsonResponse = undefined;
  }
}
