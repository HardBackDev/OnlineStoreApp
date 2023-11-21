import { Injectable } from '@angular/core';
import { environment } from '../enviroments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Category } from '../models/category';
import { Product } from '../models/product';
import { tokenGetter } from '../app.module';
import { Cart } from '../models/cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  url = `${environment.apiurl}/cart`

  constructor(private httpContext: HttpClient) { }

  public getUserCartProducts(params: string) : Observable<HttpResponse<Cart>> {
    return this.httpContext.get<Cart>(`${this.url}?${params}`, {headers: this.generateHeaders(), observe: 'response'})
  }

  public checkProductInCart(productId: string) : Observable<boolean> {
    return this.httpContext.get<boolean>(`${this.url}/checkInCart/${productId}`, {headers: this.generateHeaders()})
  }

  public addProductToCart(productId: string) {
    return this.httpContext.post(`${this.url}/${productId}`, null, {headers: this.generateHeaders()})
  }

  public deleteProductFromCart(productId: string){
    return this.httpContext.delete(`${this.url}/${productId}`, {headers: this.generateHeaders()})
  }

  private generateHeaders = () => {
    return new HttpHeaders({ 
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Authorization': `Bearer ${tokenGetter()}`})
  }
}
