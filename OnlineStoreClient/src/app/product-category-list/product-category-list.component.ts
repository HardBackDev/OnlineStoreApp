import { Component, inject } from '@angular/core';
import { Product } from '../models/product';
import { HttpResponse } from '@angular/common/http';
import { ProductService } from '../services/product.service';
import { Metadata } from '../models/metadata';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../services/category.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthenticationService } from '../services/authentication.service';
import { ParametersMetaData } from '../models/parameters-metadata';
import { range } from 'rxjs';
import { CartService } from '../services/cart-service';

@Component({
  selector: 'app-product-category-list',
  templateUrl: './product-category-list.component.html',
  styleUrls: ['./product-category-list.component.css']
})
export class ProductCategoryListComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  products: Product[] = [] 
  metadata: Metadata
  category: string
  searchedTitle: string = ''
  productParameters: any
  productParametersGroup: FormGroup = new FormGroup({});
  parametersControlls: {
    parameter: string;
    parameterName: string;
    valueType: string;
  }[] = []
  isLoadingNext: boolean
  parametersMetadata: ParametersMetaData
  orderBy: string = 'Price Desc'
  isLoading: boolean = true;
  addedProduct: Product;

  constructor(private productService: ProductService, private categoryService: CategoryService, public authService: AuthenticationService,
    public cartService: CartService, private router: Router){
  }

  ngOnInit(){
    this.category = this.route.snapshot.params['category'];
    localStorage.setItem('productUrl', 'category')
    document.onkeydown = (e) =>{
      if(e.key.toLowerCase() == 'j')
        window.scrollTo(0, 0)
    }

    this.categoryService.getCategoryParametersMetadata(this.category)
    .subscribe(paramsMetadata => {
      this.parametersMetadata = paramsMetadata

      this.categoryService.createFormGroup(this.productParametersGroup, paramsMetadata.parameters);
      this.categoryService.parseParametersToControls(this.parametersControlls, paramsMetadata)
    })

    this.getCategoriesProductsByParameters(1)

    window.addEventListener('scroll', this.scroll);
  }

  pageUp(){
    window.scrollTo(0, 0)
  }

  filterByParameters(form){
    this.productParameters = form
    this.getCategoriesProductsByParameters(1)
  }

  getCategoriesProductsByParameters(pageNumber: number){
    const params: string = this.productService.buildParametersQuery(this.productParameters)
    this.isLoading = true
    this.productService.getCategoryProducts(this.category, `pageNumber=${pageNumber}&searchTitle=${this.searchedTitle}&orderBy=${this.orderBy}&${params}`)
    .subscribe((res: HttpResponse<Product[]>) => {
      this.products = res.body
      this.metadata = JSON.parse(res.headers.get('X-Pagination'));
      this.checkProductsInCart(this.products)
      this.isLoading = false
    })
  }

  scroll = () => {
    if (!this.isLoading && this.isScrolledToBottom() && this.metadata.HasNext && !this.isLoadingNext) {
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
    const params: string = this.productService.buildParametersQuery(this.productParameters)
    this.isLoadingNext = true;

    this.productService.getCategoryProducts(this.category,
        `pageNumber=${this.metadata.CurrentPage + 1}&searchTitle=${this.searchedTitle}&${params}`)
    .subscribe((res: HttpResponse<Product[]>) =>{
      const newProducts: Product[] = res.body
      this.products = this.products.concat(newProducts);
      this.metadata = JSON.parse(res.headers.get('X-Pagination'));
      this.checkProductsInCart(res.body)
      this.isLoadingNext = false
    })
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

  filterProducts(searchTitle: string, e) {
    if(e != null)
      e.preventDefault();
    this.searchedTitle = searchTitle
    this.getCategoriesProductsByParameters(1)
  }

  selectParameterChange(event: Event, input: HTMLInputElement){
    const element = event.target as HTMLSelectElement;
    const selectedValue = element.value
    input.value = selectedValue
    this.productParametersGroup.controls[input.id].setValue(input.value)
    const dependentParameter = this.parametersMetadata.dependentSearchValues.find(p => p.dependentOnParameter == input.id)
    if(dependentParameter != null && !dependentParameter.dependentOnValues.includes(selectedValue)){
      const childElement = document.getElementById(dependentParameter.parameter) as HTMLInputElement
      childElement.value = ''
      const selectElement = document.getElementById(dependentParameter.parameter) as HTMLSelectElement
      if(selectElement != null)
        selectElement.selectedIndex = 0
      this.productParametersGroup.get(dependentParameter.parameter).setValue('')
    }
    this.filterByParameters(this.productParametersGroup.value)
  }

  orderByChange(){
    const selectedColumn = (document.getElementById('order-column') as HTMLSelectElement).value
    const direction = (document.getElementById('order-direction') as HTMLSelectElement).value

    this.orderBy = `${selectedColumn} ${direction}`
    this.filterByParameters(this.productParametersGroup.value)
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

  checkSearchParameterExist(parameter: string){
    return this.parametersMetadata.parametersSearchValues[parameter] != null || this.parametersMetadata.dependentSearchValues
    .find( p => p.parameter.toLowerCase() == parameter.toLowerCase()) != null
  }

  getParameterSearchValues(parameter: string) : any[] {
    var values: any[] = []
    if(this.parametersMetadata.parametersSearchValues[parameter] != null){
      values = values.concat(this.parametersMetadata.parametersSearchValues[parameter])
    }
    var dependentValuesParameters = this.parametersMetadata.dependentSearchValues
    .filter(p => p.parameter.toLowerCase() == parameter.toLowerCase())

    if(dependentValuesParameters.length != 0){
      for(let depndentParameter of dependentValuesParameters){
        const dependentOnParameter = depndentParameter.dependentOnParameter
        const dependentOnValue = (document.getElementById(dependentOnParameter) as HTMLInputElement).value
        if(dependentOnValue == '' || depndentParameter.dependentOnValues.includes(dependentOnValue)){
          values = values.concat(depndentParameter.searchValues)
        }
      }
    }

    return values
  }

  routeTo(action: string, category: string, id: string){
    this.router.navigate([`products/${action}/${category}/${id}`])
  }
}
