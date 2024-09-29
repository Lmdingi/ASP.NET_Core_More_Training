using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;
    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();
        if (initialize)
        {
            _countries.AddRange(new List<Country>()
            {
                new()
                {CountryID = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"), CountryName = "USA"},
                new()
                {CountryID = Guid.Parse("a7c8e9d3-2c68-4c5c-8e62-f1b1f0e7e5d8"), CountryName = "Canada"},
                new()
                {CountryID = Guid.Parse("c03f45f8-fb6c-4f8d-8b6c-5b7d8d20d6f0"), CountryName = "UK"},
                new()
                {CountryID = Guid.Parse("e7b19d9e-0043-432b-b5b4-9d8e49e87e37"), CountryName = "India"},
                new()
                {CountryID = Guid.Parse("1d3d68e5-3c62-45b5-9bcf-09d3a6c0c8b9"), CountryName = "Australia"}
            });
        }
    }
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        //Validation: countryAddRequest parameter can't be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        //Validation: CountryName can't be null
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest.CountryName));
        }

        //Validation: CountryName can't be duplicate
        if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
        {
            throw new ArgumentException("Given country name already exists");
        }

        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        //Generate CountryID
        country.CountryID = Guid.NewGuid();

        //Add country object into _country
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
        {
            return null;
        }

        Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

        if (country_response_from_list == null)
        {
            return null;
        }

        return country_response_from_list.ToCountryResponse();
    }
}
