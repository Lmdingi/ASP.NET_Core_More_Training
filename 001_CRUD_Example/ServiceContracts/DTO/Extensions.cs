using System;
using Entities;

namespace ServiceContracts.DTO;

public static class CountryExtensions
{
    //Convert from Country object to CountryResponse object
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse()
        {
            CountryID = country.CountryID,
            CountryName = country.CountryName
        };
    }
}


public static class PersonExtension
{
    /// <summary>
    /// An extension method to convert an object of Person class into PersonResponse class
    /// </summary>
    /// <param name="person">The Person object to convert</param>
    /// <returns>Returns the converted PersonResponse object</returns>
    public static PersonResponse ToPersonResponse(this Person person)
    {
        //person => PersonResponse
        return new PersonResponse()
        {
            PersonID = person.PersonID,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Address = person.Address,
            CountryID = person.CountryID,
            Gender = person.Gender,
            Age = (person.DateOfBirth != null) ? Math.Round(
                (DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
        };
    }
}
