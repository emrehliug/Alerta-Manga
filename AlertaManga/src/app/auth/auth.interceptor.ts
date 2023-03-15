
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Router } from "@angular/router";
import { tap } from "rxjs/internal/operators/tap";
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(localStorage.getItem('token') !== null){
      const cloneReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer '+ localStorage.getItem('token'))
      });
      return next.handle(cloneReq).pipe(
        tap(
          succ => {},
          err => {
            if(err.status == 401){
              this.router.navigateByUrl('user/login');
            }
            if(err.status == 400){
              this.router.navigateByUrl('user/login');
            }
          }
        )
      );
    }else{
      return next.handle(req.clone());
    }
  }
}
