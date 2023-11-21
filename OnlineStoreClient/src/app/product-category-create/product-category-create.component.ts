import { Component, inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CategoryService } from '../services/category.service';
import { ProductService } from '../services/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { ManipulatingMetdata } from '../models/manipulating-metadata';

@Component({
  selector: 'app-product-category-create',
  templateUrl: './product-category-create.component.html',
  styleUrls: ['./product-category-create.component.css']
})
export class ProductCategoryCreateComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  category: string
  productForm: FormGroup = new FormGroup({});
  productForManipulatingControlls: {
    property: string;
    type: string;
  }[] = []
  errorMessage: any;
  productMetadata: ManipulatingMetdata

  get errors(): { key: string; value: any }[] {
    return Object.entries(this.errorMessage || {}).map(([key, value]) => ({ key, value }));
  }

  constructor(private productService: ProductService, private categoryService: CategoryService, private router: Router){}

  ngOnInit(){
    this.category = this.route.snapshot.params['category'];


    this.categoryService.getCategoryManipulatingMetadata(this.category)
    .subscribe(res => {
      this.productMetadata = res
      this.categoryService.createFormGroup(this.productForm, res.manipulatingObject, res.propertiesValues);
      this.categoryService.parseManipulatingMetadataToControls(this.productForManipulatingControlls, res)
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

  selectParameterChange(event: Event, controll: string, input: HTMLInputElement){
    const element = event.target as HTMLSelectElement;
    const selectedValue = element.value
    if(input)
      input.value = selectedValue
    this.productForm.controls[controll].setValue(selectedValue)
  }

  createProduct(productForm){
    this.productService.createProduct(this.category, productForm)
    .subscribe(
    {
      next: () => {
        this.router.navigate([`/products/category/${this.category}`])
      },
      error: (err: HttpErrorResponse) => {
          this.errorMessage = err.error.errors
        }
    })
    
  }

  redirectToProducts = () => {
    this.router.navigate([`products/category/${this.category}`]);
  }
}
