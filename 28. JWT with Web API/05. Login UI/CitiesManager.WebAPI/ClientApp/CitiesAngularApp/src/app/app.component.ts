import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(public accountService: AccountService, private router: Router) { }

  onLogOutClicked() {
    this.accountService.getLogout().subscribe({
      next: (response: string) => {
        this.accountService.currentUserName = null;

        this.router.navigate([ '/login' ]);
      },

      error: (error) => {
        console.log(error);
      },

      complete: () => { },
    });
  }
}
