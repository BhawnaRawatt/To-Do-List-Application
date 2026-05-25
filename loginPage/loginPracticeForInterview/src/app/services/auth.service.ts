import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl="http://localhost:5161/api"

  constructor( private http:HttpClient) {}

  login(data:any){
    return this.http.post(`${this.apiUrl}/Login`,data);
  }

  createApi(data: any){
    return this.http.post(`${this.apiUrl}/TodoList/Create`, data);
  }

  // loadApi()
  // {
  //   return this.http.get(`${this.apiUrl}/TodoList/GetAll`)
  // }

  searchApi(searchText: any)
  {
    return this.http.get(`${this.apiUrl}/TodoList/GetData?SearchText=${searchText}`);
  }

  update(data: any)
  {
    return this.http.put(`${this.apiUrl}/TodoList/Update`,data)
  }

  delete(data:number){
    return this.http.delete(`${this.apiUrl}/TodoList/Delete?Id=${data}`)
  }
}

