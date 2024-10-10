using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;
/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonsService
{
    /// <summary>
    /// Adds a new person into the list of person
    /// </summary>
    /// <param name="personAddRequest">Person to add</param>
    /// <returns>The same person details, along with newly generated PersonID</returns>
    Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

    /// <summary>
    /// Returns all persons
    /// </summary>
    /// <returns>A list of objects of PersonResponse type</returns>
    List<PersonResponse> GetAllPersons();

    /// <summary>
    /// Returns the person object based on the given person id
    /// </summary>
    /// <param name="personID">Person id to search</param>
    /// <returns>Matching person object</returns>
    PersonResponse? GetPersonByPersonID(Guid? personID);

    /// <summary>
    /// Get all person objects that matches with the given search field and search string
    /// </summary>
    /// <param name="searchBy">Search field to search</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>All matching persons based on the given search field and search string</returns>
    List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString);

    /// <summary>
    /// Get sorted list of persons
    /// </summary>
    /// <param name="allPersons">Represents list of persons to sort</param>
    /// <param name="sortBy">Name of the property (key), based on which the should be sorted</param>
    /// <param name="sortOrder">ASC or DESC</param>
    /// <returns>Sorted persons as PersonResponse</returns>
    List<PersonResponse> GetSortedPersons
     (List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

    /// <summary>
    /// Updates the specified person details based on the given person ID
    /// </summary>
    /// <param name="personUpdateRequest">Person details to update including person id</param>
    /// <returns>The person response object after updating</returns>
    PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest);

    /// <summary>
    /// Deletes a person based on the given person id
    /// </summary>
    /// <param name="PersonID">id of Person to delete</param>
    /// <returns>true, if the deletion is successful; otherwise false</returns>
    bool DeletePerson(Guid? PersonID);

    /// <summary>
    /// gets persons as CSV
    /// </summary>
    /// <returns>the memory stream with csv</returns>
    Task<MemoryStream> GetPersonsCSV();

    /// <summary>
    /// gets persons as excel
    /// </summary>
    /// <returns>the memory stream with excel</returns>
    Task<MemoryStream> GetPersonsExcel();
}
