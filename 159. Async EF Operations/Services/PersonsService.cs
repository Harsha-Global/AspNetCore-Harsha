using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services.Helpers;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;

namespace Services
{
  public class PersonsService : IPersonsService
  {
    //private field
    private readonly PersonsDbContext _db;
    private readonly ICountriesService _countriesService;

    //constructor
    public PersonsService(PersonsDbContext personDbContext, ICountriesService countriesService)
    {
      _db = personDbContext;
      _countriesService = countriesService;
    }
    

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
      //check if PersonAddRequest is not null
      if (personAddRequest == null)
      {
        throw new ArgumentNullException(nameof(personAddRequest));
      }

      //Model validation
      ValidationHelper.ModelValidation(personAddRequest);

      //convert personAddRequest into Person type
      Person person = personAddRequest.ToPerson();

      //generate PersonID
      person.PersonID = Guid.NewGuid();

      //add person object to persons list
      _db.Persons.Add(person);
      _db.SaveChanges();
      //_db.sp_InsertPerson(person);

      //convert the Person object into PersonResponse type
      return person.ToPersonResponse();
    }


    public List<PersonResponse> GetAllPersons()
    {
      //SELECT * from Persons
      var persons = _db.Persons.Include("Country").ToList();

      return persons
        .Select(temp => temp.ToPersonResponse()).ToList();

      //return _db.sp_GetAllPersons()
      //  .Select(temp => temp.ToPersonResponse()).ToList();
    }


    public PersonResponse? GetPersonByPersonID(Guid? personID)
    {
      if (personID == null)
        return null;

      Person? person = _db.Persons.Include("Country")
        .FirstOrDefault(temp => temp.PersonID == personID);
      
      if (person == null)
        return null;

      return person.ToPersonResponse();
    }


    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
      List<PersonResponse> allPersons = GetAllPersons();
      List<PersonResponse> matchingPersons = allPersons;

      if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        return matchingPersons;


      switch (searchBy)
      {
        case nameof(PersonResponse.PersonName):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.PersonName) ?
          temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        case nameof(PersonResponse.Email):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Email) ?
          temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;


        case nameof(PersonResponse.DateOfBirth):
          matchingPersons = allPersons.Where(temp =>
          (temp.DateOfBirth != null) ?
          temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
          break;

        case nameof(PersonResponse.Gender):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Gender) ?
          temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        case nameof(PersonResponse.CountryID):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Country) ?
          temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        case nameof(PersonResponse.Address):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Address) ?
          temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        default: matchingPersons = allPersons; break;
      }
      return matchingPersons;
    }


    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
      if (string.IsNullOrEmpty(sortBy))
        return allPersons;

      List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
      {
        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

        (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

        (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

        (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

        _ => allPersons
      };

      return sortedPersons;
    }


    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
      if (personUpdateRequest == null)
        throw new ArgumentNullException(nameof(Person));

      //validation
      ValidationHelper.ModelValidation(personUpdateRequest);

      //get matching person object to update
      Person? matchingPerson = _db.Persons
        .FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
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

      _db.SaveChanges(); //UPDATE

      return matchingPerson.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personID)
    {
      if (personID == null)
      {
        throw new ArgumentNullException(nameof(personID));
      }

      Person? person = _db.Persons.FirstOrDefault(temp => temp.PersonID == personID);
      if (person == null)
        return false;

      _db.Persons.Remove(
        _db.Persons.First(temp => temp.PersonID == personID));
      _db.SaveChanges();

      return true;
    }
  }
}
