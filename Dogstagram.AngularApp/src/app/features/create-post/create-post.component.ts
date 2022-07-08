import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { CreatePostService } from '../services/create-post.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css'],
})
export class CreatePostComponent {
  fileName = '';
  img: any;
  formData: FormData = new FormData();

  constructor(
    private postService: CreatePostService,
    public dialogRef: MatDialogRef<CreatePostComponent>,
    private dom: DomSanitizer
  ) {}

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.fileName = file.name;
      this.formData.append('thumbnail', file);
      this.img = this.dom.bypassSecurityTrustUrl(URL.createObjectURL(file));
    }
  }

  post() {
    this.postService.post(this.formData).subscribe();
    this.dialogRef.close();
  }
}
