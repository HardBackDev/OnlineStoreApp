import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForLogin } from 'src/app/models/user-for-login';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { JwtToken } from '../models/jwt-token';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {
  errorMessage: string = '';
  userLoginForm: FormGroup;
  erroModalText: string = 'The wrong password or username'
  invalidLogin: boolean;
  isLoading: boolean;
  static claims: []
  
  constructor(private authService: AuthenticationService, private router: Router) { }

  ngOnInit(): void {
    this.userLoginForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  loginUser = (userForm) => {
    if (this.userLoginForm.valid)
      this.executeUserLogin(userForm);
  }

  private executeUserLogin = (userForm) => {
    const user: UserForLogin = {
      userName: userForm.userName,
      password: userForm.password
    }
    this.isLoading = true
    this.authService.loginUser(user)
    .subscribe({
      next: (response: JwtToken) => {
        const token = response.accessToken;
        localStorage.setItem("jwtToken", token);
        this.invalidLogin = false; 
        this.router.navigate(["/"]);
        this.isLoading = false
      },
      error: (err) => {
        this.invalidLogin = true
        this.isLoading = false
      }
    });
  }

  redirectToHome(){
    this.router.navigate(["/"])
  }

  validateControl = (controlName: string) => {
    if (this.userLoginForm.get(controlName).invalid && this.userLoginForm.get(controlName).touched)
      return true;
    
    return false;
  } 

  hasError = (controlName: string, errorName: string) => {
    if (this.userLoginForm.get(controlName).hasError(errorName))
      return true;
    
    return false;
  }
}
