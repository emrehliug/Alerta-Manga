import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Manga } from '../_models/Manga';
import { UserUsuario } from '../_models/UserUsuario';
import { AuthService } from '../_services/auth.service';
import { GlobalUrl } from '../_services/global';

@Component({
  selector: 'app-manga',
  templateUrl: './manga.component.html',
  styleUrls: ['./manga.component.css']
})
export class MangaComponent implements OnInit {

  mangas: Manga[] = [];
  manga: Manga;
  mangaId?: number;
  userUsuario: UserUsuario;
  
  numberPaginations = 10;
  p: number = 1; 
  contador: number = 0;
  notificacao = false;

  modoSalvar = 'post';
  currentDataHora: any;
  bodyDeletar='';
  registerForm?: FormGroup;
  readonly globalUrl = new GlobalUrl('Manga');
  readonly userUsuarioUrl = new GlobalUrl('MangaUsuario');


  constructor(
    private authService: AuthService,
    private http: HttpClient,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.getMangas();
    this.getMangasUsuario();
    this.validation();
  }

  openModal(template: any){
    this.registerForm?.reset();
    template.show();
  }

  validation(){
    this.registerForm = this.fb.group({
      id: [''],
      nome: ['', Validators.required],
      totaldeCapitulos: ['', Validators.required]
    });
  }

  verificarMangaNotificacao(nomeManga: string){
    if(this.userUsuario.manga != null && this.userUsuario.manga != undefined){
      var mangaFind = this.userUsuario.manga.find(m => m.nome.toLowerCase() == nomeManga.toLocaleLowerCase());
      this.notificacao = mangaFind !== null && mangaFind !== undefined;
    return this.notificacao;
    }  
    return false;  
  }

  updateMangaNotificacao(codigo: number, _manga: Manga){
    if(codigo == 0){
      return this.http.post(`${this.userUsuarioUrl._baseURL}/${localStorage.getItem('username')}`, _manga).
      subscribe(
      resultado => {   
        this.getMangasUsuario();
        this.getMangas();
        //this.toastr.success('Notificação alterada com sucesso.');
      },
      erro => {
        switch(erro.status) {
          case 400:
            this.toastr.error(erro.error.mensagem);
          break;
          case 404:
            this.toastr.error('Erro para alterar.');
          break;
        }
      }
    );
    }
    else {
      return this.http.delete(`${this.userUsuarioUrl._baseURL}/${localStorage.getItem('username')}/${_manga.mangaId}`).
      subscribe(
      resultado => {   
        this.getMangasUsuario();
        this.getMangas();
        //this.toastr.success('Notificação alterada com sucesso.');
      },
      erro => {
        switch(erro.status) {
          case 400:
            this.toastr.error(erro.error.mensagem);
          break;
          case 404:
            this.toastr.error('Erro para alterar.');
          break;
        }
      }
    );
    }
    

  }

  getMangas(){
    this.http.get<Manga[]>(`${this.globalUrl._baseURL}`).
    subscribe(response => {
      this.mangas = response;
    },
    err => {
      if(!this.authService.loggedIn()){
        switch(err.status) {
          case 400:
            this.toastr.error("Bad Request: Solicitação Inválida");
          break;
          case 401:
            this.toastr.error('Unauthorized: Não autorizado');
          break;
          case 404:
            this.toastr.error('Not Found:	Não encontrado');
          break;
        }
      }
    });
  }

  getMangasUsuario(){
    this.http.get<UserUsuario>(`${this.userUsuarioUrl._baseURL}/${localStorage.getItem('username')}`).
    subscribe(response => {
      this.userUsuario = response;
    },
    err => {
      if(!this.authService.loggedIn()){
        switch(err.status) {
          case 400:
            this.toastr.error("Bad Request: Solicitação Inválida");
          break;
          case 401:
            this.toastr.error('Unauthorized: Não autorizado');
          break;
          case 404:
            this.toastr.error('Not Found:	Não encontrado');
          break;
        }
      }
    });
  }

  novo(template: any){
    this.modoSalvar = 'post';
    this.registerForm?.reset();
    this.openModal(template);
  }

  salvar(template: any){
    if(this.modoSalvar == 'post'){
      console.log(this.registerForm?.value);
    this.manga = Object.assign({}, this.registerForm?.value);
    console.log(this.manga);
    console.log(this.globalUrl._baseURL);
    this.http.post(`${ this.globalUrl._baseURL }`, this.manga).
          subscribe(
          resultado => {
            template.hide();
            this.getMangas();
            this.toastr.success('Inserido com sucesso');
          },
          erro => {
            switch(erro.status) {
              case 400:
                this.toastr.error(erro.error.mensagem);
              break;
              case 404:
                this.toastr.error('Erro ao salvar.');
              break;
            }
          }
        );
    }
    else{
      this.manga = Object.assign({id: this.manga?.mangaId}, this.registerForm?.value);
      this.http.put(`${ this.globalUrl._baseURL }/` + this.manga?.mangaId ,this.manga).subscribe(
      () => {
        template.hide();
        this.getMangas();
        this.toastr.success('Atualizado com sucesso!');
      }, error => {
        this.toastr.error(`Erro ao Atualizar: ${error}`);
      }
      );
    }
  }

  editar(manga: Manga, template: any){
    this.modoSalvar = 'put';
    this.registerForm?.reset();
    this.currentDataHora = new Date();
    this.openModal(template);
    this.manga = Object.assign({}, manga);
    this.registerForm?.patchValue(manga);
    this.getMangas();
  }

  excluir(template: any) {
    return this.http.delete(`${ this.globalUrl._baseURL }/` + this.manga?.mangaId).
            subscribe(
            resultado => {
              template.hide();
              this.getMangas();
              this.toastr.success('Excluído com sucesso!');
            },
            erro => {
              switch(erro.status) {
                case 400:
                  this.toastr.error(erro.error.mensagem);
                break;
                case 404:
                  this.toastr.error('Erro ao Excluir!' + erro.error.mensagem);
                break;
              }
            }
          );
  }
}
