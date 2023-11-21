import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { AppComponent } from './app.component';
import { CategoriesListComponent } from './categories-list/categories-list.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductCategoryListComponent } from './product-category-list/product-category-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProductCategoryCreateComponent } from './product-category-create/product-category-create.component';
import { UserRegisterComponent } from './user-register/user-register.component';
import { UserLoginComponent } from './user-login/user-login.component';
import { ProductUpdateComponent } from './product-update/product-update.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductDeleteComponent } from './product-delete/product-delete.component';
import { ProductCartListComponent } from './product-cart-list/product-cart-list.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { GlobalErrorHandler } from './extensions/global-error-handler';

export function tokenGetter() { 
  return localStorage.getItem("jwtToken"); 
}

@NgModule({
  declarations: [
    UserRegisterComponent,
    UserLoginComponent,
    AppComponent,
    CategoriesListComponent,
    ProductListComponent,
    ProductCategoryListComponent,
    ProductCategoryCreateComponent,
    ProductUpdateComponent,
    ProductDetailsComponent,
    ProductDeleteComponent,
    ProductCartListComponent,
    ErrorPageComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5000", "localhost:5001"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
