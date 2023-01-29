import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  pagination: IPagination;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];

  brandIdSelected: number = 0;
  typeIdSelected: number = 0;

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getTypes() {
    this.shopService.getTypes<IType[]>().subscribe((response) => {
      this.types = [{ id: 0, name: 'All' }, ...response];
      console.log(response);
      //this.types = response;
    });
  }
  getBrands() {
    this.shopService.getBrands<IBrand[]>().subscribe((response) => {
      this.brands = [{id:0, name:'All'}, ...response]
      console.log(response);
      //this.brands = response;
    });
  }
  getProducts() {
    this.shopService
      .getProducts<IPagination>(this.brandIdSelected, this.typeIdSelected)
      .subscribe((response) => {
        console.log(response);
        this.pagination = response;
        this.products = response.data;
      });
  }

  onBrandSelected(brandId: number) {
    this.brandIdSelected = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.typeIdSelected = typeId;
    this.getProducts();
  }
}

