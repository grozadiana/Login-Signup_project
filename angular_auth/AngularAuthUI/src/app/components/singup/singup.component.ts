import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-singup',
  templateUrl: './singup.component.html',
  styleUrls: ['./singup.component.scss']
})
export class SingupComponent implements OnInit{
  type: string="password"
  isText: boolean=false;
  eyeIcon: string="fa-eye-slash"; 
  singUpForm!: FormGroup;

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router){ } 

  ngOnInit(): void {
     this.singUpForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required]
     })
  }

  hideShowPass(){
   this.isText = !this.isText;
   this.isText ? this.eyeIcon ="fa-eye" : this.eyeIcon="fa-eye-slash";
   this.isText ? this.type ="text" : this.type ="password";
}

onSingup(){
  if(this.singUpForm.valid){
    this.auth.singUp(this.singUpForm.value)
    .subscribe({
      next:(res => {
        alert(res.message);
        this.singUpForm.reset();
        this.router.navigate(['login']);
      }),
      error:(err => {
        alert(err?.error.message)
      })
    })
    console.log(this.singUpForm.value)
  } else {
    ValidateForm.validateAllFormFields(this.singUpForm);
    alert("Your form is invalide");

  }
 }
}
