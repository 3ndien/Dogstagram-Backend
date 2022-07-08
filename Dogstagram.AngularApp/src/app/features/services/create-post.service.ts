import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CreatePostService {
  private creatPostPath = environment.apiUrl + '/post/create';
  constructor(private http: HttpClient) {}

  post(data: any): Observable<any> {
    return this.http.post(this.creatPostPath, data);
  }
}
