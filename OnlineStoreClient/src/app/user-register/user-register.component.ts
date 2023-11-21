import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { UserForRegister } from '../models/user-for-register';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  errorMessage: any;
  userRegisterForm: FormGroup;
  isLoading: boolean;

  constructor(private authService: AuthenticationService, private router: Router) { }

  ngOnInit(): void {
    this.userRegisterForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      email: new FormControl('', ),
      phoneNumber: new FormControl('0',),
      password: new FormControl('', [Validators.required])
    });

  }

  get jsonItems(): { key: string; value: any }[] {
    return Object.entries(this.errorMessage || {}).map(([key, value]) => ({ key, value }));
  }

  validateControl = (controlName: string) => {
    if (this.userRegisterForm.get(controlName).invalid && this.userRegisterForm.get(controlName).touched)
      return true;
    
    return false;
  } 

  hasError = (controlName: string, errorName: string) => {
    if (this.userRegisterForm.get(controlName).hasError(errorName))
      return true;
    
    return false;
  }

  registerUser = (userForm) => {
    if (this.userRegisterForm.valid)
      this.executeUserRegistration(userForm);
  }

  private executeUserRegistration = (userForm) => {
    const user: UserForRegister = {
      userName: userForm.userName,
      password: userForm.password,
      email: userForm.email,
      phoneNumber: userForm.phoneNumber
    }
    this.isLoading = true
    this.authService.registerUser(user)
    .subscribe(
      {
        next: () => {
          this.router.navigate(['auth/login'])
          this.isLoading = false
        },
        error: (err: HttpErrorResponse) => {
            this.errorMessage = err.error;
            this.isLoading = false
          }
      })
  }

  redirectToHome = () => {
    this.router.navigate(['']);
  }
}
