import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
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

  shopParams = new ShopParams();
  totalCount = 0;
  
  sortOptions = [
    { name: 'Alphabetical' , value: 'name'},
    { name: 'Price: Low to high' , value: 'priceAsc'},
    { name: 'Price: High to low' , value: 'priceDesc'},
    
  ]

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
      .getProducts<IPagination>(this.shopParams)
      .subscribe((response) => {
        console.log(response);
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count; 
        this.products = response.data;
      });
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.getProducts();
  }

  onSortSelected(event:any){
    this.shopParams.sort = event.target.value;
    this.getProducts();
  }
}

