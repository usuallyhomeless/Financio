import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Financiouser } from 'src/app/Models/financiouser';
import { ApicallService } from 'src/app/apicall.service';
import { Debittransaction } from 'src/app/Models/debittransaction';
import { Credittransaction } from 'src/app/Models/credittransaction';
import { DashboardDetails } from 'src/app/Models/dashboard-details';
import { DebittransactionDetails } from 'src/app/Models/debittransaction-details';
import { CredittransactionDetails } from 'src/app/Models/credittransaction-details';
import { CompleteUserDetail } from 'src/app/Models/complete-user-detail';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  completeUserDetails?: CompleteUserDetail;
  User?: Financiouser;
  Debittransactions?: Debittransaction[] = [];
  AmountPaid: number = 0;
  Credittransactions?: Credittransaction[] = [];
  DashboardDetails?: DashboardDetails;
  DebittransactionDetails?: DebittransactionDetails[] = [];
  CredittransactionDetails?: CredittransactionDetails[] = [];

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

    this.service.completeUserDetailsSubject.subscribe((value) => {
      this.completeUserDetails = value;
    });
  }

  ngOnInit(): void {
    this.service.getDashboardDetails().subscribe((data: DashboardDetails) => {
      this.DashboardDetails = data;
    });

    this.service.getDebitTransactions().subscribe((data: DebittransactionDetails[]) => {
        this.DebittransactionDetails = data;
      });

    this.service.getCreditTransactions().subscribe((data: CredittransactionDetails[]) => {
        this.CredittransactionDetails = data;
      });
  }

  getAmountPaid(p: any, t: Debittransaction) {
    return (this.AmountPaid = p.cost - t.balanceleft);
  }

  getRemainingCredit() {
    var remainingCredit =
      this.DashboardDetails?.totalCredit! - this.DashboardDetails?.cardLimit!;
    return remainingCredit;
  }
}
