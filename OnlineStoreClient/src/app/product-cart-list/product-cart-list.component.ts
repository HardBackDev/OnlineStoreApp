import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Metadata } from '../models/metadata';
import { Product } from '../models/product';
import { ProductParameters } from '../models/product-parameters';
import { AuthenticationService } from '../services/authentication.service';
import { ProductService } from '../services/product.service';
import { CartService } from '../services/cart-service';
import { Cart } from '../models/cart';

@Component({
  selector: 'app-product-cart-list',
  templateUrl: './product-cart-list.component.html',
  styleUrls: ['./product-cart-list.component.css']
})

export class ProductCartListComponent {
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
  isLoading: boolean = true
  summaryPrice: number

  constructor(private service: ProductService, private cartService: CartService, private authService: AuthenticationService, 
    private router: Router){
    this._authService = authService
  }

  ngOnInit(){
    this.getProductsByParameters(1, false)
    localStorage.setItem('productUrl', 'cart')

    window.addEventListener('scroll', this.scroll);
  }

  getProductsByParameters(pageNumber: number, concat: boolean = false){
    let search = this.searchTitle ?? ''
    this.isLoading = true
    this.cartService.getUserCartProducts(`pageNumber=${pageNumber}&searchTitle=${search}&${this.service.buildParametersQuery(this.productParameters)}`)
    .subscribe((res: HttpResponse<Cart>) => {
      this.summaryPrice = res.body.summaryPrice
      this.metadata = JSON.parse(res.headers.get('X-Pagination'));
      
      if(concat){
        this.products = this.products.concat(res.body.products);
        this.isLoadingNext = false
      }
      else{
        this.products = res.body.products
        this.isLoading = false
      }
    })
  }

  filterByParameters(form){
    this.productParameters = form
    this.getProductsByParameters(1, false)
  }

  scroll = (event: any) => {
    if (!this.isLoading && this.metadata.CurrentPage < this.metadata.TotalPages && this.isScrolledToBottom() && !this.isLoadingNext) {
      this.isLoadingNext = true
      this.loadNextProducts();
    }
  };

  isScrolledToBottom(): boolean {
    const windowHeight = window.innerHeight;
    const scrollY = window.scrollY;
    const bodyHeight = document.body.clientHeight;
    return windowHeight + scrollY >= bodyHeight - 54;
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

  routeTo(action: string, category: string, id: string){
    this.router.navigate([`products/${action}/${category}/${id}`])
  }

  deleteFromCart(id: string){
    this.cartService.deleteProductFromCart(id).subscribe(res =>
      this.getProductsByParameters(1, false))
  }
}
