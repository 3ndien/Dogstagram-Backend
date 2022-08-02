import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loginPath = environment.apiUrl + '/identity/login';
  private registerPath = environment.apiUrl + '/identity/register';
  private deletePath = environment.apiUrl + '/profile/delete';

  constructor(private http: HttpClient, private router: Router) {}

  login(data: any): Observable<any> {
    return this.http.post(this.loginPath, data, {
      observe: 'response',
    });
  }

  register(data: any): Observable<any> {
    return this.http.post(this.registerPath, data, { observe: 'response' });
  }

  deleteUser(): Observable<any> {
    return this.http.delete(this.deletePath, {
      observe: 'response',
    });
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  saveUsername(username: string) {
    localStorage.setItem('username', username);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getUsername() {
    return localStorage.getItem('username');
  }

  getToken() {
    return localStorage.getItem('token');
  }
}
