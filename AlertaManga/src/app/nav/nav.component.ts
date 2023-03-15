import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public authService: AuthService,
    public router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
  }

  showMenu(){
    var result = this.router.url !== '/user/login' && this.router.url != '/user/registration';
    //result =;
    return result;
  }

  loggedIn(){
    return this.authService.loggedIn();
  }

  entrar(){
    this.router.navigate(['/user/login']);
  }

  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('permissao');
    this.toastr.show('Log Out');
    this.router.navigate(['/user/login']);
  }

  userName(){
    return localStorage.getItem('username');
  }

  permissaoSuperAdmin(){
    return this.authService.permissionSuperAdmin();
  }

}
