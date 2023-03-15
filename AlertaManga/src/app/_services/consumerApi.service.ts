import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalUrl } from './global';

@Injectable({
  providedIn: 'root'
})
export class ConsumerApiService<T> {

  globalUrl = new GlobalUrl('User/GetUser');

  constructor(private http: HttpClient, _globalUrl: GlobalUrl) {
    
  }

  getAll(): Observable<T[]> {

   return this.http.get<T[]>(this.globalUrl._baseURL);
  }

   getById(id: number): Observable<T> {
    return this.http.get<T>(this.globalUrl._baseURL + id);
   }

   post(objeto: T) {
    return this.http.post(this.globalUrl._baseURL, objeto);
   }

   put(objeto: T, id: number) {
    return this.http.put(this.globalUrl._baseURL + '?Id=' + id, objeto);
   }

   delete(id: number) {
    return this.http.delete(this.globalUrl._baseURL + '?Id=' + id);
   }
}
