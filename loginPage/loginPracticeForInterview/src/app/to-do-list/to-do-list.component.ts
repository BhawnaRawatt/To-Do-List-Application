import { Component } from '@angular/core';
import {  ReactiveFormsModule, FormGroup, Validators, FormBuilder, FormsModule } from "@angular/forms";
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-to-do-list',
  standalone: true,
  imports: [ ReactiveFormsModule , CommonModule, FormsModule],
  templateUrl: './to-do-list.component.html',
  styleUrl: './to-do-list.component.css'
})
export class ToDoListComponent {
  todolistForm! : FormGroup 
  todolist: any = [];
  SearchText : any = "";
  isEditMode = false;
  
  constructor(
    private api: AuthService,
    private fb: FormBuilder,
  ){}
  
  ngOnInit(){
    this.todolistForm = this.fb.group({
       Id: [0],
      Subject: ['',Validators.required],
      Data: ['',Validators.required] ,
    });
    // this.loadTodos();
    this.Search();
  }


  Create(){
   let param = this.todolistForm.value;
debugger
   if(this.isEditMode){
    this.api.update(param).subscribe({
        next:(res:any) =>{
          alert(res.message);
          console.log('Updated');
          this.resetForm();  
          this.Search();
        },
        error:()=> alert('Data not updated')
      });
      }

   else{this.api.createApi(param).subscribe({
    next:(res:any) => {
      alert(res.message);
      this.resetForm();
      // this.loadTodos();
      console.log(res);
      this.Search();
    },
    error:() => alert('Data not created')
   });
  }}

  // loadTodos(){
  //   debugger
  //   this.api.loadApi().subscribe({
  //     next:(res:any)=>{
  //       try{
  //         this.todolist = res?.data? JSON.parse(res.data):[];
  //       }
  //       catch(e)
  //       {
  //         console.error('Json parese error',e)
  //         this.todolist = [];
  //       }
  //     }
  //   });
  // }


  Search(){
    this.api.searchApi(this.SearchText).subscribe({
      next:(res:any) =>{
        try{
          this.todolist = res?.data?JSON.parse(res.data):[];
        }
        catch(e)
        {
          console.error("Json parse error",e)
          this.todolist = []
        }
      }
    })
  }

  resetForm(){
    this.isEditMode = false;
    this.todolistForm.reset();
  }

  edit(row:any){
    this.isEditMode = true;
    this.todolistForm.patchValue({
      Id: row.Id,
      Subject: row.Subject,
      Data: row.Data
    });        
    }
   
  remove(id: number){
    if(!confirm('Are you sure?')) return;

    this.api.delete(id).subscribe({
      next:() =>{
          alert('Deleted');
          this.Search();
      }
    });

  }
}
