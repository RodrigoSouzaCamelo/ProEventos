import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup;
  id!: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    ) {}

    ngOnInit(): void {
      this.id = this.route.snapshot.params.id;

      const formOptions: AbstractControlOptions = {
        validators: ValidatorField.MustMatch('senha', 'confirmeSenha')
      };

      this.form = this.fb.group({
        titulo: ['', Validators.required],
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        telefone: ['', [Validators.required]],
        descricao: ['', Validators.required],
        funcao: ['', Validators.required],
        senha: ['', [Validators.minLength(6), Validators.nullValidator]],
        confirmeSenha: ['', Validators.nullValidator]
      }, formOptions);
    }

    // Conveniente para pegar um FormField apenas com a letra F
    get f(): any { return this.form.controls; }

    public onSubmit(): void {
      if(this.form.invalid) return;

    }

    public resetForm(event: any): void {
      event.preventDefault();
      this.form.reset();
    }

}
