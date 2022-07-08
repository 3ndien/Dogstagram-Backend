import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { AuthService } from './authServices/auth.service';
import { AuthGuardService } from './guardServices/auth-guard.service';
import { JwtInterceptorService } from './interceptorServices/jwt-interceptor.service';

@NgModule({
  imports: [HttpClientModule],
  providers: [
    AuthService,
    AuthGuardService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptorService,
      multi: true,
    },
  ],
  exports: [],
})
export class CoreModule {}
