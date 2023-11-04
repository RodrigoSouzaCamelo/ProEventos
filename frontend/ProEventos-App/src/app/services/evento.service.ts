import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { Evento } from '../models/Evento';
import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/Pagination';

@Injectable(
  // { providedIn: 'root' }
)
export class EventoService {
  private baseURL = environment.apiUrl + '/api/evento';

  constructor(private http: HttpClient) { }

  public getEventos(page?: number, pageSize?: number): Observable<PaginatedResult<Evento[]>> {
    const paginatedResult = new PaginatedResult<Evento[]>();

    let params = new HttpParams;

    if(page != null && pageSize != null) {
      params = params.append('currentPage', page.toString());
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http
      .get<Evento[]>(`${this.baseURL}`, { observe: 'response', params })
      .pipe(
        take(3),
        map((response) => {
          paginatedResult.result = response.body ?? [];

          if(response.headers.has('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') ?? '');
          }

          return paginatedResult;
        })
      );
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http
      .get<Evento>(`${this.baseURL}/${id}`)
      .pipe(take(3));
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http
      .post<Evento>(`${this.baseURL}`, evento)
      .pipe(take(3));
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http
      .put<Evento>(`${this.baseURL}`, evento)
      .pipe(take(3));
  }

  public delete(id: number): Observable<Evento> {
    return this.http
      .delete<Evento>(`${this.baseURL}/${id}`)
      .pipe(take(3));
  }

  public postUpload(eventoId: number, file: any): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload)

    return this.http
      .post<Evento>(`${this.baseURL}/upload/image/${eventoId}`, formData)
      .pipe(take(3))
  }
}
