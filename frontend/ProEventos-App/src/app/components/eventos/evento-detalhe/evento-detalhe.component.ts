import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@environments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  modalRef?: BsModalRef;
  public form: FormGroup;
  public evento = {} as Evento;
  public loteSelecionado = {} as Lote;
  public indiceLoteSelecionado = 0;
  public imagemURL = 'assets/img/upload.png'
  public file: any | File;

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRoute: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private loteService: LoteService,
    private modalService: BsModalService) {
    this.localeService.use('pt-br');
  }

  get f(): any {
    return this.form.controls;
  }

  public modoEditar(): boolean {
    return this.evento.id !== 0 && this.evento.id !== undefined;
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
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

  get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
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

          if(this.evento.imagemURL !== '') {
            this.imagemURL = environment.apiUrl  + '/resources/images/' + this.evento.imagemURL;
          }

          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.criarLote(lote));
          })
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Não foi possível carregar evento.', 'Erro!');
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
      lotes: this.fb.array([])
    });
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  private criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm.touched };
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
          this.toastr.error('Não foi possível salvar o evento.', 'Erro');
        },
      }).add(() => this.spinner.hide());
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
          this.toastr.error('Não foi possível alterar o evento.', 'Erro');
        }
      }).add(() => this.spinner.hide());
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

  public salvarLotes(): void {
    this.spinner.show();
    if(this.form.controls.lotes.value) {
      this.loteService.put(this.evento.id, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
        },
        (error: any) => {
          this.toastr.error('Não foi possível salvar lotes.', 'Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  removerLote(template: TemplateRef<any>, indice: number) {
    this.indiceLoteSelecionado = indice;
    this.loteSelecionado.id = this.lotes.get(indice + ".id")?.value;
    this.loteSelecionado.nome = this.lotes.get(indice + ".nome")?.value;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.loteService.delete(this.evento.id, this.loteSelecionado.id).subscribe({
      next: () => {
        this.lotes.removeAt(this.indiceLoteSelecionado);
        this.toastr.success('Lote deletado com sucesso.', 'Deletado!');
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error('Não foi possível deleter lote.', 'Erro');
      },
      complete: () => {}
    }).add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }

  public mudarValor(value: Date, indice: number) {
    this.lotes.value[indice]['dataInicio'] = value;
  }

  public trazerNomeDoLote(nomeLote: string | null) {
    if(!nomeLote || nomeLote === '') return 'Nome do Lote';

    return nomeLote;
  }

  public onFileChange(event: any): void {
    const reader = new FileReader();

    reader.onload = (e: any) => this.imagemURL = e.target.result;

    this.file = event.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  private uploadImage(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.evento.id, this.file).subscribe(
      () => {
        this.carregarEventos();
        this.toastr.success('Imagem atualizada com sucesso!', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Não foi possível atualizar a imagem!', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }
}
