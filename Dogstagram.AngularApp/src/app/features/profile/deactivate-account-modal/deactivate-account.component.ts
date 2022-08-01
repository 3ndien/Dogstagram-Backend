import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/core/authServices/auth.service';

@Component({
  selector: 'app-deactivate-account',
  templateUrl: './deactivate-account.component.html',
})
export class DeactivateAccountComponent {
  constructor(
    public authService: AuthService,
    public snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<DeactivateAccountComponent>
  ) {}

  deactivateUser() {
    this.authService.deleteUser().subscribe((response) => {
      console.log(response.body['message']);
      this.dialogRef.close();
      this.snackBar.open(response.body['message'], '', { duration: 3000 });
    });
    this.authService.logout();
  }
}
