import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../services/evento.service';
import { Evento } from '../models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  //providers: [EventoService]
})
export class EventosComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];

  public ocultarImagens: boolean = false;
  public larguraImagem: number = 100;
  public marginImagem: number = 2;

  private filtroListado: string = "";

  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroListado ? this.filtrarEventos(value) : this.eventos;
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
  ) { }

  public ngOnInit(): void {
    this.getEventos();
  }

  public filtrarEventos(filtro: string): Evento[] {
    return this.eventos
      .filter(evento => evento.tema.toLocaleLowerCase().includes(filtro.toLocaleLowerCase()))
  }

  public alterarExibicaoImagens() {
    this.ocultarImagens = !this.ocultarImagens;
  }

  public getEventos(): void {
    this.eventoService.getEventos()
      .subscribe(
        (eventos: Evento[]) => {
          this.eventos = eventos;
          this.eventosFiltrados = eventos;
        },
        error => console.error(error)
      );
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

}
