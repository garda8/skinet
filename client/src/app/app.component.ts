import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'Skinet';

  constructor(private http: HttpClient){}

  products : any[];

  ngOnInit(): void{
    this.http.get('https://localhost:5001/api/products').subscribe(
      (response: any) => {
        this.products = response.data;
        console.log(response)

      }
      )

  }
}


