import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { User } from '@app/models/User';
import { AccountService } from '@app/services/account.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  constructor(private accountService: AccountService, private router: Router) {
    router.events.subscribe(val => {
      if(val instanceof NavigationEnd) {
        this.accountService.currentUser$.subscribe(value => {
            this.usuarioLogado = value !== null;
            if(this.usuarioLogado) this.usuario = value;
          })
      }
    });
  }

  isCollapse = true;
  usuarioLogado = false;
  usuario: User = {} as User;

  ngOnInit(): void {
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/user/login')
  }

  showMenu(): boolean {
    return this.router.url !== "/user/login";
  }

}
