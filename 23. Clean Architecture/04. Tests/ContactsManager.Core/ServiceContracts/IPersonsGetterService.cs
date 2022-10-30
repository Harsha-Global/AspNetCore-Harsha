using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
 /// <summary>
 /// Represents business logic (retrieve) for manipulating Perosn entity
 /// </summary>
 public interface IPersonsGetterService
 {
  /// <summary>
  /// Returns all persons
  /// </summary>
  /// <returns>Returns a list of objects of PersonResponse type</returns>
  Task<List<PersonResponse>> GetAllPersons();

  /// <summary>
  /// Returns the person object based on the given person id
  /// </summary>
  /// <param name="personID">Person id to search</param>
  /// <returns>Returns matching person object</returns>
  Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

  /// <summary>
  /// Returns all person objects that matches with the given search field and search string
  /// </summary>
  /// <param name="searchBy">Search field to search</param>
  /// <param name="searchString">Search string to search</param>
  /// <returns>Returns all matching persons based on the given search field and search string</returns>
  Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);


  /// <summary>
  /// Returns persons as CSV
  /// </summary>
  /// <returns>Returns the memory stream with CSV data of persons</returns>
  Task<MemoryStream> GetPersonsCSV();


  /// <summary>
  /// Returns persons as Excel
  /// </summary>
  /// <returns>Returns the memory stream with Excel data of persons</returns>
  Task<MemoryStream> GetPersonsExcel();
 }
}
