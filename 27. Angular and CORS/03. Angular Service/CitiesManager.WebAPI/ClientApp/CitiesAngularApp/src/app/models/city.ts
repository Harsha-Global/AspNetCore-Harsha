export class City {
  cityID: string | null;
  cityName: string | null;

  constructor(cityID: string | null = null, cityName: string | null = null) {
    this.cityID = cityID;
    this.cityName = cityName;
  }
}
