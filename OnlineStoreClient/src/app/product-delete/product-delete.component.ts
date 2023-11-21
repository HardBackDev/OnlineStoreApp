import { Component, inject } from '@angular/core';
import { Product } from '../models/product';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-product-delete',
  templateUrl: './product-delete.component.html',
  styleUrls: ['./product-delete.component.css']
})
export class ProductDeleteComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  product: Product
  category: string
  productId: string
  photoUrl: string
  productProps: {
    key: string;
    value: string;
  }[] = []

  constructor(private productService: ProductService, private router: Router){}

  ngOnInit(){
    this.category = this.route.snapshot.params['category'];
    this.productId = this.route.snapshot.params['id'];

    this.productService.getProductById(this.category, this.productId)
    .subscribe(res => {
      this.photoUrl = res.product.photoUrl
      this.parseObject(res.product)
    })
  }

  deleteProduct(){
    this.productService.deleteProduct(this.category, this.productId)
    .subscribe(
      {
        next: () => {
          this.back()
        }
      })
  }

  public parseObject(obj){
    for(var key in obj)
    {
      if(key == "photoUrl")
        continue
      this.productProps.push({ key: key, value: obj[key] })
    }
  }

  back(){
    var url: string = ''
    if(localStorage.getItem('productUrl') == 'products')
      url = '/products'
    else
      url = `/products/category/${this.category}`
    this.router.navigate([url])
  }
}
