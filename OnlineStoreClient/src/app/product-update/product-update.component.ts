import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../services/category.service';
import { ProductService } from '../services/product.service';
import { ManipulatingMetdata } from '../models/manipulating-metadata';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.css']
})
export class ProductUpdateComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  category: string;
  productForm: FormGroup = new FormGroup({});
  productControls: {
    property: string;
    type: string;
  }[] = [];
  errorMessage: any;
  productId: string;
  product: any;
  productMetadata: ManipulatingMetdata;

  get errors(): { key: string; value: any }[] {
    return Object.entries(this.errorMessage || {}).map(([key, value]) => ({ key, value }));
  }

  constructor(private productService: ProductService, private categoryService: CategoryService, private router: Router){}

  ngOnInit(){
    this.category = this.route.snapshot.params['category'];
    this.productId = this.route.snapshot.params['id'];

    this.categoryService.getCategoryManipulatingMetadata(this.category)
    .subscribe(res => {
      this.productMetadata = res
      this.categoryService.createFormGroup(this.productForm, res.manipulatingObject);
      this.categoryService.parseManipulatingMetadataToControls(this.productControls, res)

      this.productService.getProductById(this.category, this.productId)
      .subscribe(res => {
        this.product = res.product
        var props = Object.keys(res.product)
        Object.keys(this.productForm.controls).forEach((key) => {
          var formControl = this.productForm.get(key)
          var control = this.productControls.find(controll => controll.property.toLowerCase() == key.toLowerCase())
          var propKey = props.find(p => p.toLowerCase() == key.toLowerCase())

          if(control && control.type == 'date'){
            var date: string = res.product[propKey]
            date = date.substring(0, date.lastIndexOf('T'))

            formControl.setValue(date)
          }
          else{
            formControl.setValue(res.product[propKey])
          }
          var select = document.getElementById(control.property + 'Select') as HTMLSelectElement
          if(select != null){
            var selectedIndex = Array.from(select.options).findIndex(option => option.value === formControl.value.toString());
            select.selectedIndex = selectedIndex
          }
        })  
      })
    })
  }

  validateControl = (controlName: string) => {
    if (this.productForm.get(controlName).invalid && this.productForm.get(controlName).touched){
      return true;
    }
    return false;
  } 

  hasError = (controlName: string, errorName: string) => {
    if (this.productForm.get(controlName).hasError(errorName))
      return true;
    
    return false;
  }

  updateProduct(productForm){
    this.productService.updateProduct(this.category, this.productId, productForm)
    .subscribe(
      {
        next: () => {
          this.back()
        },
        error: (err: HttpErrorResponse) => {
            this.errorMessage = err.error.errors
          }
      })
  }

  selectParameterChange(event: Event, controll: string, input: HTMLInputElement){
    const element = event.target as HTMLSelectElement;
    const selectedValue = element.value
    if(input)
      input.value = selectedValue
    this.productForm.controls[controll].setValue(selectedValue)
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
