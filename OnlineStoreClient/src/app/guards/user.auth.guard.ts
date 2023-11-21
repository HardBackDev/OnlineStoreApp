import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { tokenGetter } from '../app.module';

@Injectable({
  providedIn: 'root'
})
export class UserAuthGuard {

  constructor(private router:Router, private jwtHelper: JwtHelperService){}
  
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = tokenGetter()

    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }    

    this.router.navigate([''])
    return false;
  }
}