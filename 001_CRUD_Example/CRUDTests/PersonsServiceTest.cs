using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests;

public class PersonsServiceTest
{
    //private fields
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    //constructor
    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personsService = new PersonsService();
        _countriesService = new CountriesService(false);
        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson
    //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
    [Fact]
    public void AddPerson_NullPerson()
    {
        //Arrange
        PersonAddRequest? personAddRequest = null;

        //Act
        Assert.Throws<ArgumentNullException>(() =>
        {
            _personsService.AddPerson(personAddRequest);
        });
    }

    //When we supply null value as PersonName, it should throw ArgumentException
    [Fact]
    public void AddPerson_PersonNameNull()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new() { PersonName = null };

        //Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personsService.AddPerson(personAddRequest);
        });
    }

    //When we supply proper person details, it should insert the person into persons list;
    //and it should return an object of PersonResponse, which includes with the newly generated person id
    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new()
        {
            PersonName = "Person name...",
            Email = "person@example.com",
            Address = "sample address",
            CountryID = Guid.NewGuid(),
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true
        };

        //Act
        PersonResponse person_response_from_add = _personsService.AddPerson(personAddRequest);
        List<PersonResponse> person_list = _personsService.GetAllPersons();

        //Assert
        Assert.True(person_response_from_add.PersonID != Guid.Empty);

        Assert.Contains(person_response_from_add, person_list);
    }
    #endregion

    #region GetPersonByPersonID
    //If we supply null as PersonID, it should return null as PersonResponse
    [Fact]
    public void GetPersonByPersonID_NullPersonID()
    {
        //Arrange
        Guid? personID = null;

        //Act
        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(personID);

        //Assert
        Assert.Null(person_response_from_get);
    }

    //If we supply a valid person id, it should return the person details as PersonResponse object
    [Fact]
    public void GetPersonByPersonID_WithPersonID()
    {
        //Arrange
        CountryAddRequest country_request = new() { CountryName = "Canada" };
        CountryResponse country_response = _countriesService.AddCountry(country_request);

        //Act
        PersonAddRequest person_request = new()
        {
            PersonName = "person name...",
            Email = "email@sample.com",
            Address = "address",
            CountryID = country_response.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = false
        };

        PersonResponse person_response_from_add = _personsService.AddPerson(person_request);

        PersonResponse? person_response_from_get = _personsService
        .GetPersonByPersonID(person_response_from_add.PersonID);

        //Assert
        Assert.Equal(person_response_from_add, person_response_from_get);
    }
    #endregion

    #region GetAllPersons
    //The GetAllPersons() should return an empty list by default
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        //Act
        List<PersonResponse> person_from_get = _personsService.GetAllPersons();

        //Assert
        Assert.Empty(person_from_get);
    }

    //First, we will add few persons; and then when we call GetAllPersons(), 
    //it should return the same persons that were added
    [Fact]
    public void GetAllPersons_AddFewPersons()
    {
        //Arrange
        CountryAddRequest country_request_1 = new() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new() { CountryName = "India" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request1 = new()
        {
            PersonName = "Smith",
            Email = "smith@example.com",
            Address = "address of Smith",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2002-05-06"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request2 = new()
        {
            PersonName = "Marry",
            Email = "marry@example.com",
            Address = "address of Marry",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2012-05-16"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request3 = new()
        {
            PersonName = "Rahman",
            Email = "rahman@example.com",
            Address = "address of Rahman",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2022-02-26"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new()
        {
            person_request1,
            person_request2,
            person_request3
        };

        List<PersonResponse> person_response_list_from_add = new();
        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_from_add
        _testOutputHelper.WriteLine("Expected");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }


        //Act
        List<PersonResponse> person_response_list_from_get = _personsService.GetAllPersons();

        //print person_response_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in person_response_list_from_get)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, person_response_list_from_get);
        }
    }
    #endregion

    #region GetFilteredPerson
    //If the search text is empty and search by is PersonName, it should return all persons
    [Fact]
    public void GetFilteredPerson_EmptySearchText()
    {
        //Arrange
        CountryAddRequest country_request_1 = new() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new() { CountryName = "India" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request1 = new()
        {
            PersonName = "Smith",
            Email = "smith@example.com",
            Address = "address of Smith",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2002-05-06"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request2 = new()
        {
            PersonName = "Marry",
            Email = "marry@example.com",
            Address = "address of Marry",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2012-05-16"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request3 = new()
        {
            PersonName = "Rahman",
            Email = "rahman@example.com",
            Address = "address of Rahman",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2022-02-26"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new()
        {
            person_request1,
            person_request2,
            person_request3
        };

        List<PersonResponse> person_response_list_from_add = new();
        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_from_add
        _testOutputHelper.WriteLine("Expected");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }


        //Act
        List<PersonResponse> person_response_list_from_search = _personsService
        .GetFilteredPerson(nameof(Person.PersonName), "");

        //print person_response_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in person_response_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, person_response_list_from_search);
        }
    }

    //First we will add few persons: and then we will search based on person name with some search string
    //It should return the matching persons
    [Fact]
    public void GetFilteredPerson_SearchByPersonName()
    {
        //Arrange
        CountryAddRequest country_request_1 = new() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new() { CountryName = "India" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request1 = new()
        {
            PersonName = "Smith",
            Email = "smith@example.com",
            Address = "address of Smith",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2002-05-06"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request2 = new()
        {
            PersonName = "Marry",
            Email = "marry@example.com",
            Address = "address of Marry",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2012-05-16"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request3 = new()
        {
            PersonName = "Rahman",
            Email = "rahman@example.com",
            Address = "address of Rahman",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2022-02-26"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new()
        {
            person_request1,
            person_request2,
            person_request3
        };

        List<PersonResponse> person_response_list_from_add = new();
        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_from_add
        _testOutputHelper.WriteLine("Expected");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }


        //Act
        List<PersonResponse> person_response_list_from_search = _personsService
        .GetFilteredPerson(nameof(Person.PersonName), "ma");

        //print person_response_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in person_response_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            if (person_response_from_add.PersonName != null)
            {
                if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(person_response_from_add, person_response_list_from_search);
                }
            }
        }
    }
    #endregion

    #region GetSortedPersons
    //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
    [Fact]
    public void GetSortedPersons_SearchByPersonName()
    {
        //Arrange
        CountryAddRequest country_request_1 = new() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new() { CountryName = "India" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request1 = new()
        {
            PersonName = "Smith",
            Email = "smith@example.com",
            Address = "address of Smith",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2002-05-06"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request2 = new()
        {
            PersonName = "Marry",
            Email = "marry@example.com",
            Address = "address of Marry",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2012-05-16"),
            ReceiveNewsLetters = true
        };

        PersonAddRequest person_request3 = new()
        {
            PersonName = "Rahman",
            Email = "rahman@example.com",
            Address = "address of Rahman",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("2022-02-26"),
            ReceiveNewsLetters = true
        };

        List<PersonAddRequest> person_requests = new()
        {
            person_request1,
            person_request2,
            person_request3
        };

        List<PersonResponse> person_response_list_from_add = new();
        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        //print person_response_from_add
        _testOutputHelper.WriteLine("Expected");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        List<PersonResponse> allPersons = _personsService.GetAllPersons();

        //Act
        List<PersonResponse> person_response_list_from_sort = _personsService
        .GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

        //print person_response_list_from_get
        _testOutputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in person_response_list_from_sort)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        person_response_list_from_add = person_response_list_from_add
        .OrderByDescending(temp => temp.PersonName).ToList();

        //Assert
        for (int i = 0; i < person_response_list_from_add.Count; i++)
        {
            Assert.Equal(person_response_list_from_add[i], person_response_list_from_sort[i]);
        }
    }
    #endregion

    #region UpdatePerson
    //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
    [Fact]
    public void UpdatePerson_NullPerson()
    {
        //Arrange
        PersonUpdateRequest? person_update_request = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //Act
            _personsService.UpdatePerson(person_update_request);
        });
    }

    //When we supply invalid person id, it should throw ArgumentException
    [Fact]
    public void UpdatePerson_InvalidPersonID()
    {
        //Arrange
        PersonUpdateRequest? person_update_request = new PersonUpdateRequest()
        {
            PersonID = Guid.NewGuid()
        };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _personsService.UpdatePerson(person_update_request);
        });
    }


    //When PersonName is null, it should throw ArgumentNullException
    [Fact]
    public void UpdatePerson_PersonNameIsNull()
    {
        //Arrange
        CountryAddRequest country_add_request = new() { CountryName = "UK" };
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new()
        {
            PersonName = "John",
            CountryID = country_response_from_add.CountryID,
            Address = "Abc road",
            Email = "abc@example.com",
            Gender = GenderOptions.Male
        };
        PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

        PersonUpdateRequest person_update_request =
        person_response_from_add.ToPersonUpdateRequest();
        person_update_request.PersonName = null;

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _personsService.UpdatePerson(person_update_request);
        });
    }

    //First, add a new person and try to update the person name and email
    [Fact]
    public void UpdatePerson_PersonFullDetailsUpdate()
    {
        //Arrange
        CountryAddRequest country_add_request = new() { CountryName = "UK" };
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new()
        {
            PersonName = "John",
            CountryID = country_response_from_add.CountryID,
            Address = "Abc road",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "abc@example.com",
            ReceiveNewsLetters = true,
            Gender = GenderOptions.Male
        };

        PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

        PersonUpdateRequest person_update_request =
        person_response_from_add.ToPersonUpdateRequest();
        person_update_request.PersonName = "William";
        person_update_request.Email = "william@example.com";

        //Act
        PersonResponse person_response_from_update = _personsService.UpdatePerson(person_update_request);
        PersonResponse? person_response_from_get =
        _personsService.GetPersonByPersonID(person_response_from_update.PersonID);

        //Assert
        Assert.Equal(person_response_from_get, person_response_from_update);
    }
    #endregion

    #region DeletePerson
    //If you supply an valid PersonID, it should return true
    [Fact]
    public void DeletePerson_ValidPersonID()
    {
        //Arrange
        CountryAddRequest country_add_request = new() { CountryName = "USA" };
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new()
        {
            PersonName = "Jones",
            Email = "jones@example.com",
            Address = "address of Smith",
            CountryID = country_response_from_add.CountryID,
            DateOfBirth = DateTime.Parse("2002-05-06"),
            ReceiveNewsLetters = true,
            Gender = GenderOptions.Male
        };
        PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

        //Act
        bool isDeleted = _personsService.DeletePerson(person_response_from_add.PersonID);

        //Assert
        Assert.True(isDeleted);
    }

    //If you supply an invalid PersonID, it should return false
    [Fact]
    public void DeletePerson_InvalidPersonID()
    {
        //Act
        bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());

        //Assert
        Assert.False(isDeleted);
    }
    #endregion
}
