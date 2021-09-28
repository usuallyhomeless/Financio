import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApicallService } from 'src/app/apicall.service';
import { Card } from 'src/app/Models/card';
import { CompleteUserDetail } from 'src/app/Models/complete-user-detail';
import { Location } from '@angular/common';

@Component({
  selector: 'app-completeuserdetails',
  templateUrl: './completeuserdetails.component.html',
  styleUrls: ['./completeuserdetails.component.css'],
})
export class CompleteuserdetailsComponent implements OnInit {
  userdetail?: CompleteUserDetail;
  cards?: Card;
  successMessage?: string;

  constructor(
    public service: ApicallService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
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
    var id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.service.getUserDetailsById(parseInt(id)).subscribe((data: CompleteUserDetail) => {
          this.userdetail = data;
        });

        this.service.getCardByUserId(parseInt(id)).subscribe((data: Card) => {
          this.cards = data;
          this.cards.isactive = true;
        });
    }    
  }

  activateUser() {
    if (this.cards) {
      this.service.updateCard(this.route.snapshot.params['id'], this.cards).subscribe((response) => {
          this.successMessage = 'Operation Succesfull';
          setTimeout(() => {
            this.location.back();
          }, 2000);
        });
    }
  }
}
