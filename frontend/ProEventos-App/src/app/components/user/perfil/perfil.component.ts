import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss'],
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  public imagemURL = 'assets/img/perfil.png';
  public file: any | File;

  public get ehPalestrante(): boolean {
    return this.userUpdate.funcao === "Palestrante";
  }

  constructor(private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private accountService: AccountService) {}

  ngOnInit(): void {}

  public setChangeUserForm(user: UserUpdate) {
    if(user.imagemURL) {
      this.imagemURL = environment.apiUrl  + '/resources/perfil/' + user.imagemURL;
    }

    this.userUpdate = { ...user };
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
    this.accountService.postUpload(this.file)
    .subscribe(
      () => this.toastr.success('Imagem atualizada com sucesso.', 'Sucesso'),
      (error: any) =>{
        console.error(error);
        this.toastr.error('Não foi possível fazer upload de imagem', 'Erro');
      }
    ).add(() => this.spinner.hide())
  }
}
