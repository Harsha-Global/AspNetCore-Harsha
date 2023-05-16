import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent {
  cities: City[] = [];

  constructor(private citiesService: CitiesService) {
  }

  ngOnInit() {
    this.citiesService.getCities()
      .subscribe({

        next: (response: City[]) => {
          this.cities = response;
        },

        error: (error: any) => {
          console.log(error)
        },

        complete: () => { }
      });

  }
}
