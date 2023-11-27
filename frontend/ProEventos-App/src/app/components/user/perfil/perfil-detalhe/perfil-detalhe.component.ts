import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  @Output() changeFormValue = new EventEmitter();

  form!: FormGroup;
  id!: string;
  userUpdate = {} as UserUpdate;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private accountService: AccountService,
    private palestranteService: PalestranteService
  ) {}

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
    this.emitirMudancasFormulario();
  }

  private emitirMudancasFormulario(): void {
    this.form.valueChanges.subscribe(x => this.changeFormValue.emit(x));
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser()
      .subscribe(
        (user: UserUpdate) => {
          this.userUpdate = user;
          this.form.patchValue(this.userUpdate);
          this.toastr.success('Usuário carregado.', 'Sucesso');
        },
        (error: any) => {
          console.error(error);
          this.toastr.error('Não foi possível carregar o usuário.', 'Error');
          this.router.navigate(['/dashboard'])
        }
      ).add(() => this.spinner.hide());
  }

  private validation(): void {
    this.id = this.route.snapshot.params.id;

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      userName: [''],
      imagemURL: [''],
      titulo: ['NaoInformado', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      descricao: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      password: ['', [Validators.minLength(4), Validators.nullValidator]],
      confirmePassword: ['', Validators.nullValidator]
    }, formOptions);
  }

    // Conveniente para pegar um FormField apenas com a letra F
    get f(): any { return this.form.controls; }

    public onSubmit(): void {
      //if(this.form.invalid) return;
      this.atualizarUsuario();
    }

    private atualizarUsuario(): void {
      this.userUpdate = { ...this.form.value };
      this.spinner.show();

      if(this.f.funcao.value === 'Palestrante') {
        this.palestranteService.post().subscribe(
          () => this.toastr.success('Função palestrante ativada com sucesso.', 'Sucesso'),
          (error) => {
            console.error(error);
            this.toastr.error('Erro', 'Não foi possível ativar a função palestrante.');
          }
        )
      }

      this.accountService.updateUser(this.userUpdate)
        .subscribe(
          () => this.toastr.success('Usuário atualizado.', 'Sucesso'),
          (error: any) => {
            this.toastr.error(error.error);
            console.log(error);
          }
        ).add(() => this.spinner.hide());
    }

    public resetForm(event: any): void {
      event.preventDefault();
      this.form.reset();
    }

}
