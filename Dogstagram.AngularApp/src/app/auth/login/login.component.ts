import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/authServices/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginForm = fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  login() {
    this.authService.login(this.loginForm.value).subscribe((data) => {
      console.log(data.body['message']);
      if (data.body['message'] === 'Account was restored') {
        this.snackBar.open(data.body['message'], '', {
          duration: 3000,
        });
      }
      this.authService.saveToken(data.body['token']);
      if (data.status === 200 && data.body['token'] !== undefined) {
        this.route.navigate(['/home']);
        this.authService.saveUsername(this.loginForm.value['username']);
      }
    });
  }
}
