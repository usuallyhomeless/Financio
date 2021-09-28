import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { ApicallService } from 'src/app/apicall.service';
import { CompleteUserDetail } from 'src/app/Models/complete-user-detail';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  completeUserDetails?: CompleteUserDetail;

  constructor(
    public service: ApicallService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.service.completeUserDetailsSubject.subscribe((value) => {
      this.completeUserDetails = value;
    });
  }

  ngOnInit(): void {}

  logout(): void {
    this.service.logout();
    this.router.navigate(['']);
  }
}
