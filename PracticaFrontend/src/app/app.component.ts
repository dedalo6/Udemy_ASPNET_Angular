import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  titulo: string = 'DoctorAppFrontend';
  usuarios: any;

  constructor(private http: HttpClient){}

  ngOnInit(): void {
    this.http.get('http://localhost:5074/api/Usuario').subscribe({
      next: response => this.usuarios = response,
      error: error => console.log(error),
      complete: () => console.log('La solicitud esta completa')
    })
  }
}
