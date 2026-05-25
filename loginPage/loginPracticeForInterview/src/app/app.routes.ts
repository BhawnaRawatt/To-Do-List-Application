import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ToDoListComponent } from './to-do-list/to-do-list.component';

export const routes: Routes = [
    {path: 'login', component:LoginComponent},
    {path: 'todolist', component:ToDoListComponent},
    {path: '', redirectTo:'login',pathMatch: 'full'},
];
