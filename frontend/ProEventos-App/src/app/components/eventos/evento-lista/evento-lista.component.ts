import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Lote } from '@app/models/Lote';
import { environment } from '@environments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventoId: number = 0;
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
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.getEventos();
    this.spinner.show();
  }

  public getPrimeiroLote(lotes: Lote[]): string {
    return lotes?.length > 0 ? lotes[0].nome : '';
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
      .subscribe({
        next: (eventos: Evento[]) => {
          this.eventos = eventos;
          this.eventosFiltrados = eventos;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error("Não foi possível carregar os eventos.", "Erro!");
          console.error(error);
        },
        complete: () => this.spinner.hide()
      });
  }

  openModal(event: PointerEvent | MouseEvent, template: TemplateRef<any>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventoService.delete(this.eventoId).subscribe({
      next: () => {
        this.spinner.hide();
        this.getEventos();
        this.toastr.success('Evento deletado com sucesso.', 'Deletado!');
      },
      error: (error: any) => {
        console.error(error);
        this.spinner.hide();
        this.toastr.error('Não foi possível deleter evento.', 'Erro');
      },
      complete: () => {}
    });
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`/eventos/detalhe/${id}`]);
  }

  mostrarImagem(imagemURL: string): string {
    return imagemURL ? environment.apiUrl + "/resources/images/" + imagemURL :
      'assets/img/semImagem.jpeg'
  }
}
