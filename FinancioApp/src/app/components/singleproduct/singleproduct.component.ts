import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ApicallService } from 'src/app/apicall.service';
import { Product } from 'src/app/Models/product';
import { Scheme } from 'src/app/Models/scheme';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { JsonResponse } from 'src/app/Models/common';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { CompleteUserDetail } from 'src/app/Models/complete-user-detail';
import { DebitTransactionModel } from 'src/app/Models/debittransactionmodel';

@Component({
  selector: 'app-singleproduct',
  templateUrl: './singleproduct.component.html',
  styleUrls: ['./singleproduct.component.css'],
})
export class SingleproductComponent implements OnInit {
  product?: Product;
  allscheme?: Scheme[];
  checkProductBuy?: JsonResponse;
  checkProductPay?: JsonResponse;
  completeUserDetails?: CompleteUserDetail;
  productEmi?: number;
  buyResponse?: JsonResponse;
  payResponse?: JsonResponse;
  debitTransaction?: DebitTransactionModel;

  buyProductForm = new FormGroup({
    schemeid: new FormControl('', [Validators.required]),
  });

  constructor(
    public service: ApicallService,
    private route: ActivatedRoute,
    private router: Router,
    private modalService: NgbModal
  ) {    
    var routeString: string = '';
    this.route.snapshot.url.forEach((element) => {
      routeString = routeString + '/' + element.path;
    });

    this.completeUserDetails = this.service.completeUserDetailsSubject.getValue();
    this.service.completeUserDetailsSubject.subscribe((response) => {
      this.completeUserDetails = response;
    });

    this.service.isLoggedin().subscribe((response) => {
      if (!response) {
        this.router.navigate([`/login/${encodeURIComponent(routeString)}`]);
      }
    });
  }

  ngOnInit(): void {
    var id = this.route.snapshot.paramMap.get('id');

    if (id != null) {
      var productid = parseInt(id);
      this.service.getProductById(productid).subscribe((data: Product) => {
          this.product = data;
          this.calcEMI();
        });

      this.service.getAllScheme().subscribe((response) => {
        this.allscheme = response;
      });

      this.service.getCheckProductBuy(productid).subscribe((response) => {
          this.checkProductBuy = response;
        });

      this.service.getCheckProductPay(productid).subscribe((response) => {
          this.checkProductPay = response;
          if(this.checkProductPay.status == 200 || this.checkProductPay.status == 422){
            this.service.getDebitTransactionbyAuth(productid).subscribe(response => {
              this.debitTransaction = response;
            })
          }
        });
    }
  }

  getSchemeDuration(): Scheme | undefined {
    return this.allscheme?.filter((x) => x.id == this.buyProductForm.value.schemeid)[0];
  }

  calcEMI(): void {
    var cost = this.product?.cost;
    var duration = this.getSchemeDuration()?.schemeduration;
    if (cost != undefined && duration != undefined)
      this.productEmi = Math.round((cost / duration) * 100) / 100;
    else if (cost != undefined)
      this.productEmi = Math.round((cost / 12) * 100) / 100;
  }

  buynow(): void {
    if (this.product?.id) {
      this.service.postbuyproduct(this.product?.id, this.buyProductForm.value.schemeid).subscribe((response) => {
          this.buyResponse = response;
          if(response.status == 200){
            setTimeout(() => {
              window.location.reload();
            }, 2000);
          }
        });
    }
  }

  paynow(): void {
    if(this.product?.id){
      this.service.postPayProduct(this.product?.id).subscribe(response => {
        this.payResponse = response;        
      });
      setTimeout(() => {
        window.location.reload();
      }, 2000);
    }
  }

  open(content: any) {
    this.modalService.open(content);
  }

  closeAlert() {
    this.buyResponse = undefined;
    this.payResponse = undefined;
  }
}





