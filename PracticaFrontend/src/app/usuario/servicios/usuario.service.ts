import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Login } from '../interfaces/login';
import { Sesion } from '../interfaces/sesion';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  baseUrl: string = environment.apiUrl+'usuario/'

  constructor(private http: HttpClient) { }

  iniciarSesion(request: Login): Observable<Sesion>{
    return this.http.post<Sesion>(`${this.baseUrl}login`,request);
  }
}
