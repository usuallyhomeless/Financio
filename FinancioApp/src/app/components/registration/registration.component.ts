import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Validators } from '@angular/forms';
import { CardType } from '../../Models/cardtype';
import { ApicallService } from '../../apicall.service';
import { Bank } from 'src/app/Models/bank';
import { Ifsc } from 'src/app/Models/Ifcs';
import { UserAndCard } from 'src/app/Models/userandcard';
import { JsonResponse } from 'src/app/Models/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
})
export class RegistrationComponent implements OnInit {
  allcardtype?: CardType[];
  allbank?: Bank[];
  applicableIfsc: Ifsc[] = [];
  serverJsonResponse?: JsonResponse;

  registrationForm = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.pattern('[a-zA-Z].{0,50}'),
    ]),
    dob: new FormControl('', [Validators.required]),
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.email,
    ]),
    phone: new FormControl('', [
      Validators.required,
      Validators.maxLength(13),
      Validators.pattern('[- +()0-9]+'),
    ]),
    username: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.maxLength(30),
      Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$'),
    ]),
    repassword: new FormControl('', [
      Validators.required,
      Validators.maxLength(30),
      this.pwdMatchValidator,
    ]),
    address: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    cardtype: new FormControl('', [Validators.required]),
    bank: new FormControl('', [Validators.required]),
    ifsc: new FormControl('', [Validators.required]),
    accountnumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(30),
      Validators.pattern('[0-9].{0,30}'),
    ]),
  });

  pwdMatchValidator(frm: any) {
    return frm?.parent?.controls?.password?.value ===
      frm?.parent?.controls?.repassword?.value
      ? null
      : { mismatch: true };
  }

  constructor(private service: ApicallService, private router: Router) {}

  ngOnInit(): void {
    this.service.getAllCardType().subscribe((response) => {
      this.allcardtype = response;
    });

    this.service.getAllBank().subscribe((response) => {
      this.allbank = response;
    });
  }

  OnSubmit(): void {
    this.serverJsonResponse = undefined;
    if (this.registrationForm.valid) {
      var x: UserAndCard = new UserAndCard();
      var formvalue = this.registrationForm.value;
      x.name = formvalue.name;
      x.phone = formvalue.phone;
      x.email = formvalue.email;
      x.username = formvalue.username;
      x.password = formvalue.password;
      x.dob = formvalue.dob;
      x.address = formvalue.address;
      x.cardtype = formvalue.cardtype;
      x.bank = parseInt(formvalue.bank);
      x.accountnumber = formvalue.accountnumber;
      x.ifsc = parseInt(formvalue.ifsc);
      this.service.postCreateUserAndCard(x).subscribe((response) => {
        this.serverJsonResponse = response;
        if (this.serverJsonResponse.status == 200) {
          setTimeout(() => {
            this.router.navigate(["/login"]);
          }, 2000);
        }
      });
    } else {
      this.serverJsonResponse = {
        status: 400,
        message: 'Invalid form please check all fields',
      };
    }
  }

  selectbank(bankid: number): void {
    this.service.getIfcsByBankId(bankid).subscribe((response) => {
      this.applicableIfsc = response;
    });
  }

  closeAlert(): void {
    this.serverJsonResponse = undefined;
  }
}
