using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    //private field
    private readonly PersonsDbContext _db;
    private readonly ICountriesService _countriesService;
    private readonly ILogger<PersonsService> _logger;
     private readonly IDiagnosticContext _diagnosticContext;

    //constructor
    public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService,
    ILogger<PersonsService> logger, IDiagnosticContext diagnosticContext)
    {
        _db = personsDbContext;
        _countriesService = countriesService;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.Country = person.Country.CountryName;

        return personResponse;
    }

    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
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
        await _db.Persons.AddAsync(person);
        await _db.SaveChangesAsync();

        //convert the Person object into PersonResponse type
        return ConvertPersonToPersonResponse(person);

    }

    public List<PersonResponse> GetAllPersons()
    {
        _logger.LogInformation("GetAllPersons of PersonsService");
        var persons = _db.Persons.Include("Country").ToList();

        return _db.Persons.ToList()
        .Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
    }

    public PersonResponse? GetPersonByPersonID(Guid? personID)
    {
        if (personID == null)
        {
            return null;
        }

        Person? person = _db.Persons.Include("Country")
        .FirstOrDefault(temp => temp.PersonID == personID);

        if (person == null)
        {
            return null;
        }

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString)
    {
        _logger.LogInformation("GetFilteredPerson of PersonsService");
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

        _diagnosticContext.Set("matchingPersons", matchingPersons);

        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons
    (List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        _logger.LogInformation("GetSortedPersons of PersonsService");
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
        Person? matchingPerson = _db.Persons.FirstOrDefault(
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

        _db.SaveChanges();
        return ConvertPersonToPersonResponse(matchingPerson);
    }

    public bool DeletePerson(Guid? personID)
    {
        if (personID == null)
        {
            throw new ArgumentNullException(nameof(personID));
        }

        Person? person = _db.Persons.FirstOrDefault(temp => temp.PersonID == personID);

        if (person == null)
        {
            return false;
        }

        _db.Persons.Remove(_db.Persons.First(temp => temp.PersonID == personID));

        _db.SaveChanges();
        return true;
    }

    public async Task<MemoryStream> GetPersonsCSV()
    {
        MemoryStream memoryStream = new();
        StreamWriter streamWriter = new(memoryStream);

        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture);
        CsvWriter csvWriter = new(streamWriter, csvConfiguration);

        csvWriter.WriteField(nameof(PersonResponse.PersonName));
        csvWriter.WriteField(nameof(PersonResponse.Email));
        csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        csvWriter.WriteField(nameof(PersonResponse.Age));
        csvWriter.WriteField(nameof(PersonResponse.Gender));
        csvWriter.WriteField(nameof(PersonResponse.Country));
        csvWriter.WriteField(nameof(PersonResponse.Address));
        csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
        csvWriter.NextRecord();

        List<PersonResponse> persons = _db.Persons.Include("Country")
        .Select(temp => temp.ToPersonResponse()).ToList();

        foreach (var person in persons)
        {
            csvWriter.WriteField(person.PersonName);
            csvWriter.WriteField(person.Email);
            if (person.DateOfBirth.HasValue)
            {
                csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-mm-dd"));
            }
            else
            {
                csvWriter.WriteField("");
            }
            csvWriter.WriteField(person.Age);
            csvWriter.WriteField(person.Country);
            csvWriter.WriteField(person.Address);
            csvWriter.WriteField(person.ReceiveNewsLetters);
            csvWriter.NextRecord();
            csvWriter.Flush();
        }

        memoryStream.Position = 0;
        return memoryStream;

    }

    public async Task<MemoryStream> GetPersonsExcel()
    {
        MemoryStream memoryStream = new();
        using (ExcelPackage excelPackage = new(memoryStream))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
            worksheet.Cells["A1"].Value = "Persons Name";
            worksheet.Cells["B1"].Value = "Email";
            worksheet.Cells["C1"].Value = "D.O.B";
            worksheet.Cells["D1"].Value = "Age";
            worksheet.Cells["E1"].Value = "Gender";
            worksheet.Cells["F1"].Value = "Country";
            worksheet.Cells["G1"].Value = "Address";
            worksheet.Cells["H1"].Value = "Receive News Letters";

            int row = 2;
            List<PersonResponse> persons = _db.Persons
            .Include("Country").Select(temp => temp.ToPersonResponse()).ToList();

            foreach (var person in persons)
            {
                worksheet.Cells[row, 1].Value = person.PersonName;
                worksheet.Cells[row, 2].Value = person.Email;
                if (person.DateOfBirth.HasValue)
                {
                    worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString();
                }
                worksheet.Cells[row, 4].Value = person.Age;
                worksheet.Cells[row, 5].Value = person.Gender;
                worksheet.Cells[row, 6].Value = person.Country;
                worksheet.Cells[row, 7].Value = person.Address;
                worksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                row++;
            }
            worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

            await excelPackage.SaveAsync();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}
