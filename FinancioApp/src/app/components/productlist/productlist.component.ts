import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/Models/product';
import { ApicallService } from 'src/app/apicall.service';


@Component({
  selector: 'app-productlist',
  templateUrl: './productlist.component.html',
  styleUrls: ['./productlist.component.css']
})
export class ProductlistComponent implements OnInit {

  products: Product[] = [];
  constructor(public service: ApicallService) { }

  ngOnInit(): void {
    this.service.getAllProduct().subscribe((data: Product[])=>{
      this.products = data;
    });
  }
  
}
