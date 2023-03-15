import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs';
import { GlobalUrl } from './global';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  globalUrl = new GlobalUrl('User/');
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  login(model: any){
    return this.http
      .post(this.globalUrl._baseURL + "login", model).pipe(
        map((Response: any) => {
          const user = Response;
          if(user){
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            localStorage.setItem('username', user.username);
            localStorage.setItem('permissao', user.permissao);
          }
        })
    );
  }

  register(model: any){
    return this.http
      .post(this.globalUrl._baseURL + "register", model);
  }

  loggedIn(){
    const token = localStorage.getItem('token')?.toString();
    return !this.jwtHelper.isTokenExpired(token == undefined ? "" : token);
  }

  permissionSuperAdmin()
  {
    const access = localStorage.getItem('permissao')?.toString();
    return access?.toLowerCase() == "SuperAdmin".toLowerCase();
  }

}
