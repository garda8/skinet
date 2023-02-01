import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { IPagination } from '../shared/models/pagination';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProducts<T>(shopParams : ShopParams) {
  //getProducts<T>(brandId?: number, typeId?: number, sort?: string) {
    let params = new HttpParams();
    if (shopParams.brandId>0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId>0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }
    params = params.append('sort', shopParams.sort);
    
    return this.http.get<T>(this.baseUrl + 'products', { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
      })
    )
  }

  getBrands<T>() {
    return this.http.get<T>(this.baseUrl + 'products/brands');
  }

  getTypes<T>() {
    return this.http.get<T>(this.baseUrl + 'products/types');
  }
}
