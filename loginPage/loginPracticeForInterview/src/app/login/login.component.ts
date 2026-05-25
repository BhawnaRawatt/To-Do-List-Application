import { Component } from '@angular/core';
import {  FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginform! : FormGroup;

  constructor(
    private apiservice: AuthService,
    private fb: FormBuilder,
    private router: Router
  ){}

  ngOnInit():void {
    this.loginform = this.fb.group({
    username: ['', Validators.required],
    password: [ '', Validators.required]
  });
}

  login(){
    debugger
      let param  = this.loginform.value;
      this.apiservice.login(param).subscribe({
        next:(res: any) => {
          alert(res.message);
          this.router.navigate(['/todolist']);
          console.log(res);
        },
        error: () => alert('Invaild Credential')
        
      });
  }
}
