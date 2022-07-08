import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/authServices/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent {
  public username: any;
  constructor(private authService: AuthService, private snackBar: MatSnackBar) {
    this.username = this.authService.getUsername();
  }

  deactivateUser() {
    this.authService.deleteUser().subscribe((response) => {
      console.log(response.body['message']);

      this.snackBar.open(response.body['message'], '', { duration: 3000 });
    });
    this.authService.logout();
  }
}
