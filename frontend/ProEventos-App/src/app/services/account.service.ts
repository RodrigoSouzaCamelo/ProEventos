import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/User';
import { UserUpdate } from '@app/models/UserUpdate';
import { environment } from '@environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  private baseURL = environment.apiUrl + '/api/account';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(`${this.baseURL}/login`, model)
    .pipe(
        take(3),
        map(user => {
          if(user) this.setCurrentUser(user);
        })
    );
  }

  public register(model: any): Observable<void> {
    return this.http.post<User>(`${this.baseURL}/register`, model)
      .pipe(
        take(3),
        map(user => {
          if(user) this.setCurrentUser(user);
        })
      );
  }

  public logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(undefined);
  }

  public getUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(`${this.baseURL}/user`)
      .pipe(take(3));
  }

  public updateUser(model: UserUpdate): Observable<void> {
    return this.http.put<UserUpdate>(`${this.baseURL}/user`, model)
      .pipe(
        take(3),
        map((user: UserUpdate) => this.setCurrentUser(user))
      )
  }

  public setCurrentUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

}
