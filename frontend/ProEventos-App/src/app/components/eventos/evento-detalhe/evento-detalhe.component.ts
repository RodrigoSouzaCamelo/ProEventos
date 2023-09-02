import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  public form: FormGroup;
  public evento = {} as Evento;

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRoute: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router) {
    this.localeService.use('pt-br');
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEventos();
  }

  public carregarEventos(): void {
    const eventoIdParam = parseInt(this.activatedRoute.snapshot.paramMap.get('id') ?? '0');

    if(eventoIdParam !== null && eventoIdParam > 0) {
      this.eventoService.getEventoById(eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento.', 'Erro!')
          console.error(error);
        },
        complete: () => {
          this.spinner.hide();
        }
      });
    }
  }

  private validation(): void {
    this.form = this.fb.group({
      id: [0],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      data: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: ['', Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public criarNovoEvento(): void {
    this.evento = { ...this.form.value };
      this.eventoService.post(this.evento).subscribe({
        next: () => {
          this.toastr.success('Evento salvo com sucesso.', 'Sucesso!');
          this.irParaTelaDeListaDeEventos();
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Não foi possível salvar o evento.', 'Erro');
        },
        complete: () => this.spinner.hide()
      });
  }

  public irParaTelaDeListaDeEventos(): void {
    this.router.navigate([`/eventos/lista`]);
  }

  public alterarEvento() {
      this.eventoService.put(this.evento).subscribe({
        next: () => {
          this.toastr.success('Evento alterado com sucesso.', 'Sucesso!');
          this.irParaTelaDeListaDeEventos();
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Não foi possível alterar o evento.', 'Erro');
        },
        complete: () => this.spinner.hide()
      });
  }

  public salvarAlteracoes(): void {
    this.spinner.show();
    this.evento = { ...this.form.value };

    if(this.form.valid && this.evento.id === 0) {
      this.criarNovoEvento();
    } else if(this.form.valid && this.evento.id !== 0) {
      this.alterarEvento();
    }
  }
}
