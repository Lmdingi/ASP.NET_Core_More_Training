using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    //private field
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    //constructor
    public PersonsService(bool initialize = true)
    {
        _persons = new();
        _countriesService = new CountriesService();

        if (initialize)
        {
            _persons.AddRange(new List<Person>(){
                new(){Address="",PersonName = ""},
                new()
                {
                    PersonID = Guid.Parse("d7a12f7e-2a84-4f52-a118-e8c2a8b3f49f"),
                    CountryID = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                    PersonName = "Saw",
                    Email ="sridehalgh0@miibeian.gov.cn",
                    DateOfBirth = DateTime.Parse("6/15/1997"),
                    Gender = "Male",
                    Address = "98 Waubesa Trail",
                    ReceiveNewsLetters = false,
                },
                 new()
                {
                    PersonID = Guid.Parse("5b7c7c94-01c8-46b7-91a1-0d9e7b7a6ee3"),
                    CountryID = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                    PersonName = "Marja",
                    Email ="mmacwhirter1@fc2.com",
                    DateOfBirth = DateTime.Parse("1/13/2007"),
                    Gender="Female",
                    Address="0503 Derek Lane",
                    ReceiveNewsLetters= false
                },
                new()
                {
                    PersonID = Guid.Parse("8d97c1b3-bd95-470c-8981-4b9b29034c19"),
                    CountryID = Guid.Parse("a7c8e9d3-2c68-4c5c-8e62-f1b1f0e7e5d8"),
                    PersonName="Elsbeth",
                    Email="ecubberley2@webs.com",
                    DateOfBirth= DateTime.Parse("12/3/2014"),
                    Gender="Female",
                    Address="64781 East Alley",
                    ReceiveNewsLetters= false
                },
                new()
                {
                    PersonID = Guid.Parse("a6d70a5f-7569-4c0b-a1cb-ef0156c9931d"),
                    CountryID = Guid.Parse("a7c8e9d3-2c68-4c5c-8e62-f1b1f0e7e5d8"),
                    PersonName= "Kalvin",
                    Email= "kcarass3@w3.org",
                    DateOfBirth = DateTime.Parse("1/1/2000"),
                    Gender ="Male",
                    Address = "8 Sommers Place",
                    ReceiveNewsLetters = true
                },
                new()
                {
                    PersonID = Guid.Parse("2f7e9f8e-dc0e-41a7-8d47-183e9c1e0c7c"),
                    CountryID = Guid.Parse("c03f45f8-fb6c-4f8d-8b6c-5b7d8d20d6f0"),
                    PersonName ="Baird",
                    Email= "bmcgiffin4@networksolutions.com",
                    DateOfBirth=  DateTime.Parse("9/24/2019"),
                    Gender="Male",
                    Address= "91 Kings Pass",
                    ReceiveNewsLetters = true
                },
                 new()
                {
                    PersonID = Guid.Parse("b08efc9b-6d4e-4b72-9c3c-d72f3c1e9e11"),
                    CountryID = Guid.Parse("c03f45f8-fb6c-4f8d-8b6c-5b7d8d20d6f0"),
                    PersonName= "Clarita",
                    Email= "cfedorchenko5@bravesites.com",
                    DateOfBirth= DateTime.Parse("9/18/2005"),
                    Gender= "Female",
                    Address= "248 Fallview Junction",
                    ReceiveNewsLetters= false
                },
                  new()
                {
                    PersonID = Guid.Parse("6c1e7b48-f4a9-4e5a-b1ec-77c0b673b147"),
                    CountryID = Guid.Parse("e7b19d9e-0043-432b-b5b4-9d8e49e87e37"),
                    PersonName= "Sherwood",
                    Email= "skelcey6@jigsy.com",
                    DateOfBirth = DateTime.Parse("5/27/2024"),
                    Gender = "Male",
                    Address = "2 Miller Circle",
                    ReceiveNewsLetters= false
                },
                  new()
                {
                    PersonID = Guid.Parse("e1b2e3f9-9a2d-4a71-8ff6-c3f3d9a0b642"),
                    CountryID = Guid.Parse("e7b19d9e-0043-432b-b5b4-9d8e49e87e37"),
                    PersonName= "Violante",
                    Email ="vowtram7@creativecommons.org",
                    DateOfBirth= DateTime.Parse("5/16/2001"),
                    Gender= "Female",
                    Address= "4147 Lyons Plaza",
                    ReceiveNewsLetters= false
                },
                  new()
                {
                    PersonID = Guid.Parse("ec43c3a0-9ec2-4672-94d1-063f33e60a28"),
                    CountryID = Guid.Parse("1d3d68e5-3c62-45b5-9bcf-09d3a6c0c8b9"),
                    PersonName= "Kin",
                    Email= "kommundsen8@home.pl",
                    DateOfBirth= DateTime.Parse("1/11/2007"),
                    Gender= "Male",
                    Address= "5541 Mesta Hill",
                    ReceiveNewsLetters= false
                },
                new()
                {
                    PersonID = Guid.Parse("db2f3a8a-5645-4b0e-bb55-25cb174e9db8"),
                    CountryID = Guid.Parse("1d3d68e5-3c62-45b5-9bcf-09d3a6c0c8b9"),
                    PersonName= "Anselm",
                    Email= "afayerman9@redcross.org",
                    DateOfBirth=DateTime.Parse("10/19/2004"),
                    Gender= "Male",
                    Address= "886 Twin Pines Pass",
                    ReceiveNewsLetters= false
                }
            });
        }
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;

        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {

        //check if PersonAddRequest is not null
        if (personAddRequest == null)
        {
            throw new ArgumentNullException(nameof(personAddRequest));
        }

        //Model Validate
        ValidationHelper.ModelValidation(personAddRequest);

        //convert personAddRequest into Person type
        Person person = personAddRequest.ToPerson();

        //generate PersonID
        person.PersonID = Guid.NewGuid();

        //add person object to persons list
        _persons.Add(person);

        //convert the Person object into PersonResponse type
        return ConvertPersonToPersonResponse(person);

    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
    }

    public PersonResponse? GetPersonByPersonID(Guid? personID)
    {
        if (personID == null)
        {
            return null;
        }

        Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

        if (person == null)
        {
            return null;
        }

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        {
            return matchingPersons;
        }

        switch (searchBy)
        {
            case nameof(PersonResponse.PersonName):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName)) ?
                 temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email)) ?
                     temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ?
                     temp.DateOfBirth.Value.ToString("dd MMMM yyyy")
                     .Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ?
                     temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(PersonResponse.CountryID):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country)) ?
                     temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(PersonResponse.Address):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address)) ?
                     temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            default: matchingPersons = allPersons; break;
        }

        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons
    (List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            return allPersons;
        }

        List<PersonResponse> sortedPersons = (sortBy, sortOrder)
        switch
        {
            (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),


            (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),


            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),


            (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.Age).ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.Age).ToList(),


            (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),


            (nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),


            (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),


            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) =>
            allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) =>
            allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

            _ => allPersons
        };

        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest)
    {
        if (personUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(Person));
        }

        //validation
        ValidationHelper.ModelValidation(personUpdateRequest);

        //get matching person object to update
        Person? matchingPerson = _persons.FirstOrDefault(
            temp => temp.PersonID == personUpdateRequest.PersonID
        );

        if (matchingPerson == null)
        {
            throw new ArgumentException("Given person id doesn't exist");
        }

        //update all details
        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryID = personUpdateRequest.CountryID;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return ConvertPersonToPersonResponse(matchingPerson);
    }

    public bool DeletePerson(Guid? personID)
    {
        if (personID == null)
        {
            throw new ArgumentNullException(nameof(personID));
        }

        Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

        if (person == null)
        {
            return false;
        }

        _persons.RemoveAll(temp => temp.PersonID == personID);
        return true;
    }
}
