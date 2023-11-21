import { Component } from '@angular/core';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.css']
})
export class ErrorPageComponent {
  status: string
  message: string
  apiMessage: string

  constructor() {}

  ngOnInit(){
    this.status = localStorage.getItem('errorStatus')
    this.message = localStorage.getItem('errorMessage')
    this.apiMessage = localStorage.getItem('apiError')
  }
}
