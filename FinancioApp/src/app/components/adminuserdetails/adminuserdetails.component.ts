import { Component, OnInit } from '@angular/core';
import { Financiouser } from 'src/app/Models/financiouser';
import { ApicallService } from 'src/app/apicall.service';
import { Card } from 'src/app/Models/card';
import { Ng2SearchPipe } from 'ng2-search-filter';
import { CompleteUserDetail } from 'src/app/Models/complete-user-detail';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-adminuserdetails',
  templateUrl: './adminuserdetails.component.html',
  styleUrls: ['./adminuserdetails.component.css'],
})
export class AdminuserdetailsComponent implements OnInit {
  users: Financiouser[] = [];
  cards: Card[] = [];

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

  ngOnInit(): void {
    this.service.getAllUser().subscribe((data: Financiouser[]) => {
      this.users = data;
    });

    this.service.getAllCard().subscribe((data: Card[]) => {
      this.cards = data;
    });
  }
  
  getcardActivation(user : Financiouser): Boolean {
    return this.cards.filter(x => x.financiouser == user.id)[0]?.isactive
  }
}



