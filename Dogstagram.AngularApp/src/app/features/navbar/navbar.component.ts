import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from 'src/app/core/authServices/auth.service';
import { CreatePostComponent } from '../create-post/create-post.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  constructor(private authService: AuthService, public dialog: MatDialog) {}

  openDialog(): void {
    const dialogRef = this.dialog.open(CreatePostComponent);
    dialogRef;
  }

  logout() {
    this.authService.logout();
  }
}
