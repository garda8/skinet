import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { IPagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProducts<T>(brandId?: number, typeId?: number) {
    let params = new HttpParams();
    if (brandId) {
      params = params.append('brandId', brandId.toString());
    }
    if (typeId) {
      params = params.append('typeId', typeId.toString());
    }

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
