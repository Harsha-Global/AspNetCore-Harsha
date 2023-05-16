import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  putCityForm: FormGroup;
  editCityID: string | null = null;

  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [ Validators.required ])
    });

    this.putCityForm = new FormGroup({
      cities: new FormArray([])
    });
  }

  get putCityFormArray() : FormArray {
    return this.putCityForm.get("cities") as FormArray;
  }

  loadCities() {
    this.citiesService.getCities()
      .subscribe({

        next: (response: City[]) => {
          this.cities = response;

          this.cities.forEach(city => {
            this.putCityFormArray.push(new FormGroup({
              cityID: new FormControl(city.cityID, [Validators.required]),
              cityName: new FormControl({ value: city.cityName, disabled: true }, [Validators.required]),
            }));
          });
        },

        error: (error: any) => {
          console.log(error)
        },

        complete: () => { }
      });
  }


  ngOnInit() {
    this.loadCities();
  }


  get postCity_CityNameControl(): any {
    return this.postCityForm.controls['cityName'];
  }


  public postCitySubmitted() {
    this.isPostCityFormSubmitted = true;

    console.log(this.postCityForm.value);

    this.citiesService.postCity(this.postCityForm.value).subscribe({
      next: (response: City) => {
        console.log(response);

        //this.loadCities();
        this.cities.push(new City(response.cityID, response.cityName));

        this.postCityForm.reset();
        this.isPostCityFormSubmitted = false;
      },

      error: (error: any) => {
        console.log(error);
      },

      complete: () => { }
    });
  }

  //Executes when the clicks on 'Edit' button the for the particular city
  editClicked(city: City) : void {
    this.editCityID = city.cityID;
  }

  //executes when the clicks on 'Update' button after editing
  updateClicked(i: number): void {

    this.citiesService.putCity(this.putCityFormArray.controls[i].value).subscribe({
      next: (response: string) => {
        console.log(response);

        this.editCityID = null;

        this.putCityFormArray.controls[i].reset(this.putCityFormArray.controls[i].value);
      },

      error: (error: any) => {
        console.log(error);
      },

      complete: () => {},
    });
  }
}
