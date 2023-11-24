import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Palestrante } from '@app/models/Palestrante';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { PalestranteService } from '@app/services/palestrante.service';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss'],
})
export class PalestranteListaComponent implements OnInit {
  public palestrantes: Palestrante[] = [];
  public pagination = {} as Pagination;
  private termoBuscaChanged = new Subject<string>();

  constructor(
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      pageSize: 3,
      totalCount: 1
    } as Pagination;

    this.getPalestrantes();
  }

  public getPalestrantes(): void {
    this.spinner.show();

    this.palestranteService.getPalestrantes(this.pagination)
      .subscribe({
        next: (response: PaginatedResult<Palestrante[]>) => {
          this.palestrantes = response.result;
          this.pagination = response.pagination;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error("Não foi possível carregar os palestrantes.", "Erro!");
          console.error(error);
        },
        complete: () => this.spinner.hide()
      });
  }

  public filtrarPalestrantes(event?: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe((filtrarPor) => {
        this.spinner.show();
        this.palestranteService
          .getPalestrantes(this.pagination, filtrarPor)
          .subscribe({
            next: (response: PaginatedResult<Palestrante[]>) => {
              this.palestrantes = response.result;
              this.pagination = response.pagination;
            },
            error: (error: any) => {
              this.spinner.hide();
              this.toastr.error(
                'Não foi possível carregar os Palestrantes.',
                'Erro!'
              );
              console.error(error);
            },
            complete: () => this.spinner.hide(),
          });
      });
    }

    this.termoBuscaChanged.next(event.value);
  }
}
