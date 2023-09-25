import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable()
export class LoteService {
  private baseURL = 'http://localhost:5000/api/lote';
  constructor(private http: HttpClient) { }

  public getLotesByEventoId(eventoId: number): Observable<Lote[]> {
    return this.http
      .get<Lote[]>(`${this.baseURL}/${eventoId}`)
      .pipe(take(3));
  }

  public put(eventoId: number, lote: Lote): Observable<Lote> {
    return this.http
      .put<Lote>(`${this.baseURL}/${eventoId}`, lote)
      .pipe(take(3));
  }

  public delete(eventoId: number, loteId: number): Observable<Lote> {
    return this.http
      .delete<Lote>(`${this.baseURL}/${loteId}/evento/${eventoId}`)
      .pipe(take(3));
  }
}
