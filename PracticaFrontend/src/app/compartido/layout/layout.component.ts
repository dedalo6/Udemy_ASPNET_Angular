import { Component, OnInit } from '@angular/core';
import { CompartidoService } from '../compartido.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit{

  username: string = ''

  constructor(private router: Router, private compartidoService: CompartidoService)
  {

  }
  ngOnInit(): void {
    const usuarioToken = this.compartidoService.obtenerSesion();
    if (usuarioToken != null) {
      this.username = usuarioToken.userName;
    }
  }

  cerrarSesion()
  {
    this.compartidoService.eliminarSesion();
    this.router.navigate(['login']);
  }

}
