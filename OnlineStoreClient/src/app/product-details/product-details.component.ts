import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { AuthenticationService } from '../services/authentication.service';
import { CategoryService } from '../services/category.service';
import { CartService } from '../services/cart-service';
import { HttpErrorResponse } from '@angular/common/http';
import { ProductDetails } from '../models/product-details';
import { Product } from '../models/product';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  category: string
  productId: string
  photoUrl: string
  productProps: {
    key: string;
    value: string;
  }[] = []
  showMessage: boolean
  errorMessage: string
  isLoading: boolean = true;
  title: string = '';
  price: string = '';
  productInCart: boolean = false

  constructor(private productService: ProductService, public authService: AuthenticationService, private router: Router,
    private cartService: CartService){
  }

  ngOnInit(){
    this.productId = this.route.snapshot.params['id'];
    this.category = this.route.snapshot.params['category'];
    this.isLoading = true;

    this.productService.getProductById(this.category, this.productId)
    .subscribe(res => {
      this.title = res.product.title
      this.price = res.product.price
      this.photoUrl = res.product.photoUrl
      this.parseObject(res)
      this.checkProductsInCart();
      this.isLoading = false;
    })
  }

  checkProductsInCart(){
    if(!this.authService.isUserAuthenticated())
      return
    this.cartService.checkProductInCart(this.productId)
    .subscribe(inCart =>{
      this.productInCart = inCart
    })
  }

  deleteProductFromCart(){
    this.isLoading = true;
    if(!this.productInCart)
      return
    this.cartService.deleteProductFromCart(this.productId)
    .subscribe(r => {
      this.productInCart = false
      this.isLoading = false;
    })
  }

  addToCart(){
    this.isLoading = true;
    this.cartService.addProductToCart(this.productId)
    .subscribe({
      next: () => {
        this.showMessage = true
        this.productInCart = true
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message
        this.isLoading = false;
      }
    })
  }

  public parseObject(productDetails: ProductDetails){
    for(var prop in productDetails.product)
    {
      if(prop == "photoUrl" || prop == "id" || prop == 'title' || prop == 'price' || prop == 'category')
        continue

      let propertyName = productDetails.propertiesNames[prop.toUpperCase()] ?? prop
      let value = productDetails.product[prop]
      if(value.toString().substring(10) == 'T00:00:00'){
        value = value.substring(0, 10)
      }

      this.productProps.push({ key: propertyName, value: value })
    }
  }

  back(){
      var url: string = ''
      if(localStorage.getItem('productUrl') == 'products')
        url = '/products'
      else if(localStorage.getItem('productUrl') == 'category')
        url = `/products/category/${this.category}`
      else if(localStorage.getItem('productUrl') == 'cart')
        url = '/products/cart'
      this.router.navigate([url])
    }

}
