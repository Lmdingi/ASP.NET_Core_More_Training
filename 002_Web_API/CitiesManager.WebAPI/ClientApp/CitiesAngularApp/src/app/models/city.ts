export class City {
    cityID: number | null;
    cityName: string | null;
    
    constructor(cityID: number | null = null, cityName: string | null = null){
        this.cityID = cityID;
        this.cityName = cityName;
    }
}
