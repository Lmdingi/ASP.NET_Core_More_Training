using System;
using ServiceContracts.DTO;

namespace ServiceContracts;
/// <summary>
/// Represent business logic for manipulating Country entity
/// </summary>
public interface ICountriesService
{
    /// <summary>
    /// Adds a country object to the list of countries
    /// </summary>
    /// <param name="countryAddRequest">Country object to add</param>
    /// <returns>The country object after adding it (including newly generated country id) adding it</returns>
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

    /// <summary>
    /// Returns all countries from the list
    /// </summary>
    /// <returns>All countries from the list of CountryResponse</returns>
    List<CountryResponse> GetAllCountries();
    
    /// <summary>
    /// Returns a country object based on the given country id
    /// </summary>
    /// <param name="countryID">CountryID (guid) to search/param>
    /// <returns>Matching country as CountryResponse object</returns>
    CountryResponse? GetCountryByCountryID(Guid? countryID);
}

