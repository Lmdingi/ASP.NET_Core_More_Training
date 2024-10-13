import { Component } from '@angular/core';
import { CitiesService } from '../service/cities.service';
import { City } from '../models/city';

@Component({
  selector: 'app-cities',
  standalone: true,
  imports: [],
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent {
  cities: City[] = [];

  constructor(private citiesService: CitiesService){

  }

  ngOnInit() {
    this.citiesService.getCities().subscribe({
      next: (response: City[]) => {
        this.cities = response;
      },

      error: (error: any) => {
        console.log(error)
      },
      complete: () => {}
    })
  }
}
