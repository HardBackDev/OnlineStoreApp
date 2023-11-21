import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesListComponent } from './categories-list/categories-list.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductCategoryListComponent } from './product-category-list/product-category-list.component';
import { ProductCategoryCreateComponent } from './product-category-create/product-category-create.component';
import { AdminAuthGuard } from './guards/admin.auth.guard';
import { UserRegisterComponent } from './user-register/user-register.component';
import { UserLoginComponent } from './user-login/user-login.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductUpdateComponent } from './product-update/product-update.component';
import { ProductDeleteComponent } from './product-delete/product-delete.component';
import { ProductCartListComponent } from './product-cart-list/product-cart-list.component';
import { UserAuthGuard } from './guards/user.auth.guard';
import { ErrorPageComponent } from './error-page/error-page.component';

const routes: Routes = [
  {path: '', component: CategoriesListComponent},
  {path: 'products', component: ProductListComponent},
  {path: 'products/category/:category', component: ProductCategoryListComponent},
  {path: 'products/category/:category/create', component: ProductCategoryCreateComponent, canActivate: [AdminAuthGuard]},
  {path: 'products/category/:category/:id', component: ProductDetailsComponent},
  {path: 'products/update/:category/:id', component: ProductUpdateComponent, canActivate: [AdminAuthGuard]},
  {path: 'products/delete/:category/:id', component: ProductDeleteComponent, canActivate: [AdminAuthGuard]},
  {path: 'auth', component: UserRegisterComponent},
  {path: 'auth/login', component: UserLoginComponent},
  {path: 'products/cart', component: ProductCartListComponent, canActivate: [UserAuthGuard]},
  {path: 'error', component: ErrorPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
