import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Evento } from '../models/Evento';
import { environment } from '@environments/environment';

@Injectable(
  // { providedIn: 'root' }
)
export class EventoService {
  private baseURL = environment.apiUrl + '/api/evento';

  constructor(private http: HttpClient) { }

  public getEventos(): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseURL}`)
      .pipe(take(3));
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http
      .get<Evento>(`${this.baseURL}/${id}`)
      .pipe(take(3));
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseURL}/tema/${tema}`)
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
