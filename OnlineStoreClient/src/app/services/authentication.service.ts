import { Injectable } from '@angular/core';
import { environment } from '../enviroments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Category } from '../models/category';
import { FormBuilder, FormGroup } from '@angular/forms';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserForLogin } from '../models/user-for-login';
import { UserForRegister } from '../models/user-for-register';
import { JwtToken } from '../models/jwt-token';
import { tokenGetter } from '../app.module';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  url = `${environment.apiurl}/auth`

  constructor(private httpContext: HttpClient, private jwtHelper: JwtHelperService) { }
  
  public registerUser = (user: UserForRegister) => {
    return this.httpContext.post<UserForRegister>(this.url, user, this.generateHeaders());
  }

  public loginUser = (user: UserForLogin) => {
    return this.httpContext.post<JwtToken>(`${this.url}/login`, user, this.generateHeaders());
  }

  public refreshToken = (token: JwtToken) => {
    return this.httpContext.post<JwtToken>(`token/refresh`, token, this.generateHeaders())
  }

  public getRole = () => {
    if(!this.isUserAuthenticated())
      return null
    const decodedToken = this.jwtHelper.decodeToken(tokenGetter());
    const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    return role
  }

  public getUserName = () => {
    if(!this.isUserAuthenticated())
        return null
    const decodedToken = this.jwtHelper.decodeToken(tokenGetter());
    const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
    return userName
      
  }

  public isUserAdmin = (): boolean => {
    if(!this.isUserAuthenticated())
        return false
    return this.getRole() == 'Admin'

  }

  public isUserAuthenticated = (): boolean => {
    const token = tokenGetter()
    
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json'
      })
    }
  }
}
