import { Component } from '@angular/core';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';
import { HttpResponse } from '@angular/common/http';
import { Metadata } from '../models/metadata';
import { FormControl, FormGroup } from '@angular/forms';
import { ProductParameters } from '../models/product-parameters';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { CartService } from '../services/cart-service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
[x: string]: any;
  products: Product[] = [] 
  metadata: Metadata
  productParameters: ProductParameters
  productParametersGroup: FormGroup = new FormGroup({
    minPrice: new FormControl(0),
    maxPrice: new FormControl(100_000_000)
  });
  searchTitle: string
  _authService: AuthenticationService
  isLoadingNext: boolean
  isLoading: boolean = true;
  addedProduct: Product;

  constructor(private service: ProductService, private authService: AuthenticationService, private router: Router, private cartService: CartService){
    this._authService = authService
  }

  ngOnInit(){
    this.getProductsByParameters(1, false)
    localStorage.setItem('productUrl', 'products')

    window.addEventListener('scroll', this.scroll);
  }

  pageUp(){
    window.scrollTo(0, 0)
  }

  getProductsByParameters(pageNumber: number, concat: boolean = false){
    if(concat)
      this.isLoadingNext = true
    let search = this.searchTitle ?? ''
    this.service.getAllProducts(`pageNumber=${pageNumber}&searchTitle=${search}&${this.service.buildParametersQuery(this.productParameters)}`)
    .subscribe((res: HttpResponse<Product[]>) => {
      if(concat){
        this.products = this.products.concat(res.body);
        this.isLoadingNext = false
      }
      else{
        this.products = res.body
      }
      this.checkProductsInCart(this.products)
      this.metadata = JSON.parse(res.headers.get('X-Pagination'));
      this.isLoading = false
    })
  }

  filterByParameters(form){
    this.productParameters = form
    this.getProductsByParameters(1, false)
  }

  scroll = (event: any) => {
    if (!this.isLoading && this.metadata.HasNext && this.isScrolledToBottom() && !this.isLoadingNext) {
      this.isLoadingNext = true
      this.loadNextProducts();
    }
  };

  isScrolledToBottom(): boolean {
    const scrollY = window.scrollY + document.body.clientHeight;
    const height = document.body.scrollHeight;

    return scrollY >= height - 10;
  }

  loadNextProducts(): void {
    this.getProductsByParameters(this.metadata.CurrentPage + 1, true)
  }

  filterProducts(searchTitle: string, e) {
    e.preventDefault();
    this.searchTitle = searchTitle
    this.getProductsByParameters(1, false)
  }

  onChange(event){
    const input = (event.target as HTMLInputElement);
    
    const price = Number.parseInt(input.value)
    if(price < 0)
      input.value = '0'
    else if(price > 2000000000)
      input.value = '2000000000'
  }

  checkProductsInCart(products: Product[]){
    if(!this.authService.isUserAuthenticated())
      return
    for(let product of products){
      this.cartService.checkProductInCart(product.id)
      .subscribe(inCart =>{
        product.inCart = inCart
      })
    }
  }

  deleteProductFromCart(product: Product){
    if(!product.inCart){
      return
    }

    product.inCart = null

    this.cartService.deleteProductFromCart(product.id)
    .subscribe(r => {
      product.inCart = false
    })
  }

  addProductToCart(product: Product){
    if(product.inCart){
      return
    }
    product.inCart = null
    this.cartService.addProductToCart(product.id)
    .subscribe(r =>{
      this.addedProduct = product
      product.inCart = true
    });
  }

  routeTo(action: string, category: string, id: string){
    console.log(`products/${action}/${category}/${id}`)
    this.router.navigate([`products/${action}/${category}/${id}`])
  }
}
