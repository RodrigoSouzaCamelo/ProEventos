import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable(
  // { providedIn: 'root' }
)
export class EventoService {
  private baseURL = 'http://localhost:5000/api';
  constructor(private http: HttpClient) { }

  public getEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/evento`);
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/evento/${id}`);
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/evento/tema/${tema}`);
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http.post<Evento>(`${this.baseURL}/evento`, evento);
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/evento`, evento);
  }

  public delete(id: number): Observable<Evento> {
    return this.http.delete<Evento>(`${this.baseURL}/evento/${id}`);
  }
}
