import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/authServices/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { DeactivateAccountComponent } from './deactivate-account-modal/deactivate-account.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent {
  public username: any;
  constructor(private authService: AuthService, private dialog: MatDialog) {
    this.username = this.authService.getUsername();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DeactivateAccountComponent);
    dialogRef;
  }
}
