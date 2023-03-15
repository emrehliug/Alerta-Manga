import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { HomeComponent } from './home/home.component';
import { LogComponent } from './logManga/log.component';
import { MangaComponent } from './manga/manga.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  {path: 'user', component: UserComponent,
    children:
    [
      {path: 'login', component: LoginComponent}
     ,{path: 'registration', component: RegistrationComponent}
    ]
   }
  ,{path: 'home', component: HomeComponent, canActivate: [AuthGuard]}
  ,{path: 'mangas', component: MangaComponent, canActivate: [AuthGuard]}
  ,{path: 'logs', component: LogComponent, canActivate: [AuthGuard]}
  ,{path: '', redirectTo: 'home', pathMatch: 'full'}
  ,{path: '**', redirectTo: 'home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
