import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

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
    private router: ActivatedRoute,
    private eventoService: EventoService) {
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
    const eventoIdParam = parseInt(this.router.snapshot.paramMap.get('id') ?? '0');

    if(eventoIdParam !== null && eventoIdParam > 0) {
      this.eventoService.getEventoById(eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        error: (error: any) => console.error(error),
        complete: () => {}
      });
    }
  }

  private validation(): void {
    this.form = this.fb.group({
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
}
