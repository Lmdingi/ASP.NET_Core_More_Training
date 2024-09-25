using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;
    public CountriesService()
    {
        _countries = new List<Country>();
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

        if(country_response_from_list == null){
            return null;
        }

        return country_response_from_list.ToCountryResponse();
    }
}
