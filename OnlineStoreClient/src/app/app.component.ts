import { Component, Renderer2, Type } from '@angular/core';
import { Product } from './models/product';
import { AuthenticationService } from './services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'OnlineStoreClient';
  _authService: AuthenticationService

  constructor(private renderer: Renderer2, private authService: AuthenticationService){
    this._authService = authService
  }
  
  ngOnInit(){
    const bodyElement = document.body;
    this.renderer.setStyle(bodyElement, 'overflow-x', 'hidden');
    
  }

  logOut = () => {
    localStorage.removeItem("jwtToken");
  }
}
