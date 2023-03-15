import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { template } from 'lodash';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Log } from '../_models/Log';
import { AuthService } from '../_services/auth.service';
import { GlobalUrl } from '../_services/global';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.css']
})
export class LogComponent implements OnInit {

  statusLimparLog = false;

  logsBot: Log[] = [];
  logsApi: Log[] = [];

  numberPaginations = 5;
  p: number = 1;
  numberPaginationsApi = 5;
  pApi: number = 1;


  readonly globalUrl = new GlobalUrl('Log/');
  readonly globalUrlBot = new GlobalUrl('Log/logBot');
  readonly globalUrlApi = new GlobalUrl('Log/logApi');

  constructor(
    private authService: AuthService,
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.getLogsBot();
    this.getLogsApi();

  }

  openModal(template: any){
    template.show();
  }

  novo(template: any){
    this.openModal(template);
  }

  fechar(template: any){
    template.Hide();
  }

  getLogsBot(){
    this.http.get<Log[]>(`${this.globalUrlBot._baseURL}`).
    subscribe(response => {
      this.logsBot = response;
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

  getLogsApi(){
    this.http.get<Log[]>(`${this.globalUrlApi._baseURL}`).
    subscribe(response => {
      this.logsApi = response;
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

  deleteLogs(){
    this.statusLimparLog = true;
    return this.http.delete(`${this.globalUrl._baseURL}`).
      subscribe(
      resultado => {
        this.getLogsBot();
        this.getLogsApi();
        this.statusLimparLog = false;
        this.fechar("bs-modal");
      },
      erro => {
        switch(erro.status) {
          case 400:
            this.toastr.error(erro.error.mensagem);
          break;
          case 404:
            this.toastr.error('Erro para deletar Logs.');
          break;
        }
      }
    );
  }
}
