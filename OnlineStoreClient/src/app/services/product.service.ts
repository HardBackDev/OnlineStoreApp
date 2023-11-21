import { Injectable } from '@angular/core';
import { environment } from '../enviroments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Category } from '../models/category';
import { Product } from '../models/product';
import { tokenGetter } from '../app.module';
import { ProductDetails } from '../models/product-details';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  url = `${environment.apiurl}/products`

  constructor(private httpContext: HttpClient) { }

  public getAllProducts(params: string) : Observable<HttpResponse<Product[]>> {
    return this.httpContext.get<Product[]>(`${this.url}?${params}`, { observe: 'response' })
  }

  public getCategoryProducts(category: string, params: string) : Observable<HttpResponse<Product[]>> {
    return this.httpContext.get<Product[]>(`${this.url}/${category}?${params}`, { observe: 'response' })
  }

  public getProductById(category: string, id: string) : Observable<ProductDetails> {
    return this.httpContext.get<ProductDetails>(`${this.url}/${category}/${id}`, this.generateHeaders())
  }

  public createProduct(category: string, product: any) {
    return this.httpContext.post(`${this.url}/${category}`, product, this.generateHeaders())
  }

  public updateProduct(category: string, id: string, product: any) {
    return this.httpContext.put(`${this.url}/${category}/${id}`, product, this.generateHeaders())
  }

  public deleteProduct(category: string, id: string){
    return this.httpContext.delete(`${this.url}/${category}/${id}`, this.generateHeaders())
  }

  buildParametersQuery(productParameters):string {
    let query = ""
    for (const key in productParameters) {
      if (productParameters.hasOwnProperty(key)) {
        if(productParameters[key] != "")
          query += `${key}=${productParameters[key]}&`
      }
    }
    query += "pageSize=12"
    return query
  }

  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': `Bearer ${tokenGetter()}`})
    }
  }
}
