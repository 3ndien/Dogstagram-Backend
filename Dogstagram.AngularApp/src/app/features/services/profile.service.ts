import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private getAllPath = environment.localHost + '/post/getallfiles';
  private profileDetailsPath = environment.localHost + '/profile/details';

  constructor(private http: HttpClient) {}

  getAllFiles(): Observable<any> {
    return this.http.get(this.getAllPath, {
      observe: 'body',
      responseType: 'json',
    });
  }

  profileDetails(): Observable<any> {
    return this.http.get(this.profileDetailsPath);
  }
}
