import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {
  private baseURL = environment.apiUrl + '/api/palestrante';

  constructor(private http: HttpClient) { }

  public getPalestrantes(pagination: Pagination, term?: string): Observable<PaginatedResult<Palestrante[]>> {
    const { currentPage, pageSize,  } = pagination;
    const paginatedResult = new PaginatedResult<Palestrante[]>();

    let params = new HttpParams;

    if(currentPage != null && pageSize != null) {
      params = params.append('currentPage', currentPage.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    if(term) {
      params = params.append('term', term);
    }

    return this.http
      .get<Palestrante[]>(`${this.baseURL}/todos`, { observe: 'response', params })
      .pipe(
        take(3),
        map((response) => {
          paginatedResult.result = response.body ?? [];

          if(response.headers.has('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') ?? '');
          }

          paginatedResult.result = paginatedResult.result.map(result => {

            if(result.user.imagemURL)
              result.user.imagemURL = environment.apiUrl +'/resources/perfil/'+ result.user.imagemURL;
            else
              result.user.imagemURL = './assets/img/perfil.png';

            return result;
          })

          return paginatedResult;
        })
      );
  }

  public getpalestrante(): Observable<Palestrante> {
    return this.http
      .get<Palestrante>(`${this.baseURL}`)
      .pipe(take(3));
  }

  public post(): Observable<Palestrante> {
    return this.http
      .post<Palestrante>(`${this.baseURL}`, { } as Palestrante)
      .pipe(take(3));
  }

  public put(palestrante: Palestrante): Observable<Palestrante> {
    return this.http
      .put<Palestrante>(`${this.baseURL}`, palestrante)
      .pipe(take(3));
  }
}
