import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { tokenGetter } from '../app.module';

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard {

  constructor(private router:Router, private jwtHelper: JwtHelperService){}
  
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = tokenGetter()
    const decodedToken = this.jwtHelper.decodeToken(tokenGetter());
    const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']

    if (token && !this.jwtHelper.isTokenExpired(token) && role == 'Admin'){
      return true;
    }    

    this.router.navigate([''])
    return false;
  }
}