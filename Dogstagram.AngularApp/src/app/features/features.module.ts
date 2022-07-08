import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { MaterialModule } from '../material.module';
import { FeaturesRoutingModule } from './features-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { CreatePostService } from './services/create-post.service';

@NgModule({
  declarations: [NavbarComponent, ProfileComponent, CreatePostComponent],
  imports: [CommonModule, MaterialModule, FeaturesRoutingModule],
  providers: [CreatePostService],
  exports: [NavbarComponent],
})
export class FeaturesModule {}
