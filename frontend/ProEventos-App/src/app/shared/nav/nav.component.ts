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

  constructor(public accountService: AccountService, private router: Router) {
    router.events.subscribe(val => {
      if(val instanceof NavigationEnd) {
        const user = JSON.parse(localStorage.getItem('user') ?? '');

        if(user) this.accountService.setCurrentUser(user);
      }
    });
  }

  isCollapse = true;

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
