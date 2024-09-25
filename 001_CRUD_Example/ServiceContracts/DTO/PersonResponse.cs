using System;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;
/// <summary>
/// Response DTO class that is used as return type of most methods of Persons Service
/// </summary>
public class PersonResponse
{
    public Guid PersonID { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public bool? ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }

    /// <summary>
    /// Compare the current object data with the parameter object
    /// </summary>
    /// <param name="obj">The PersonResponse Object to compare</param>
    /// <returns>True or False, indicating wether all person details are matched with the specified object</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(PersonResponse))
        {
            return false;
        }

        PersonResponse person = (PersonResponse)obj;

        return PersonID == person.PersonID &&
        PersonName == person.PersonName &&
        Email == person.Email &&
        DateOfBirth == person.DateOfBirth &&
        Gender == person.Gender &&
        CountryID == person.CountryID &&
        Address == person.Address &&
        ReceiveNewsLetters == person.ReceiveNewsLetters;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $@"
        PersonID: {PersonID}
        PersonName: {PersonName},
        Email: {Email},
        DateOfBirth: {DateOfBirth},
        Gender: {Gender},
        CountryID: {CountryID},
        Country: {Country},
        Address: {Address},
        ReceiveNewsLetters: {ReceiveNewsLetters},
        Age: {Age} ";
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonID = PersonID,
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
            Address = Address,
            CountryID = CountryID,
            ReceiveNewsLetters = ReceiveNewsLetters
        };
    }

}
