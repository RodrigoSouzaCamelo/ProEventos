import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any[] = [];
  public eventosFiltrados: any[] = [];
  ocultarImagens: boolean = false;
  larguraImagem: number = 100;
  marginImagem: number = 2;
  private _filtroLista: string = "";

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this._filtroLista ? this.filtrarEventos(value) : this.eventos;
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  filtrarEventos(filtro: string): any {
    return this.eventos
      .filter(evento => evento.tema.toLocaleLowerCase().includes(filtro.toLocaleLowerCase()))
  }

  alterarExibicaoImagens() {
    this.ocultarImagens = !this.ocultarImagens;
  }

  public getEventos(): void {
    this.http.get<any[]>('https://localhost:5001/api/evento')
      .subscribe(
        response => {
          this.eventos = response;
          this.eventosFiltrados = response;
        },
        error => console.error(error)
      );
  }

}
