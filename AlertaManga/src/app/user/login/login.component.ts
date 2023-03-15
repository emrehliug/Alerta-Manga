import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: any = {};
  constructor(private authService: AuthService,
              private toastr: ToastrService,
              public router: Router) { }

  ngOnInit() {
    if(localStorage.getItem('token') !== null){
      this.router.navigate(['/home']);
    }
  }

  login(){
    this.authService.login(this.model)
    .subscribe(
      () => {
        this.router.navigate(['/home']);
      },error =>{
          this.toastr.error('Falha ao tentar Logar');
      }
    );
  }

}
