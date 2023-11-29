import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss'],
})
export class PalestranteDetalheComponent implements OnInit {
  public form!: FormGroup;
  public situacaoDoForm = '';
  public corDaDescricao = '';

  public get f(): any {
    return this.form.controls;
  }

  constructor(
    private formBuilder: FormBuilder,
    private palestranteService: PalestranteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.validation();
    this.verificaForm();
  }

  private verificaForm() {
    this.form.valueChanges
      .pipe(
        map(() => {
          this.situacaoDoForm = 'Minicurrículo está sendo atualizado!';
          this.corDaDescricao = 'text-warning';
        }),
        debounceTime(1000),
        tap(() => this.spinner.show())
      )
      .subscribe(() => {
        this.palestranteService.put({ ...this.form.value }).subscribe(
          () => {
            this.situacaoDoForm = 'Minicurrículo foi atualizado!';
            this.corDaDescricao = 'text-success';
          },
          (error) => {
            console.error(error);
            this.toastr.error('Erro ao atualizar o minicurrículo', 'Erro');
          }
          ).add(() => this.spinner.hide());
        });
  }

  private validation() {
    this.form = this.formBuilder.group({
      miniCurriculo: [''],
    });
  }
}
