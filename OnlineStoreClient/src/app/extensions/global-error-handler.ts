import { ErrorHandler, Injectable, NgZone } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable({
    providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
    constructor(private router: Router) {}

    handleError(error: any) {
        localStorage.removeItem('errorMessage')
        localStorage.removeItem('apiError')
        localStorage.removeItem('errorStatus')
        
        localStorage.setItem('errorMessage', error.message || 'An unexpected error occurred.')

        console.error('Error from global error handler', error);
        if (error instanceof HttpErrorResponse) {
            localStorage.setItem('apiError', error.error.message)
            localStorage.setItem('errorStatus', error.error.status || 500)
        }

        this.router.navigate(['error'])
    }
}