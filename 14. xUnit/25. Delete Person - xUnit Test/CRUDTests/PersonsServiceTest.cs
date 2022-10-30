using System;
using System.Collections.Generic;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;
using System.Linq;

namespace CRUDTests
{
  public class PersonsServiceTest
  {
    //private fields
    private readonly IPersonsService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    //constructor
    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
      _personService = new PersonsService();
      _countriesService = new CountriesService();
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
        _personService.AddPerson(personAddRequest);
      });
    }


    //When we supply null value as PersonName, it should throw ArgumentException
    [Fact]
    public void AddPerson_PersonNameIsNull()
    {
      //Arrange
      PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

      //Act
      Assert.Throws<ArgumentException>(() =>
      {
        _personService.AddPerson(personAddRequest);
      });
    }

    //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
      //Arrange
      PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person name...", Email = "person@example.com", Address = "sample address", CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01"), ReceiveNewsLetters = true };

      //Act
      PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);

      List<PersonResponse> persons_list = _personService.GetAllPersons();

      //Assert
      Assert.True(person_response_from_add.PersonID != Guid.Empty);

      Assert.Contains(person_response_from_add, persons_list);
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
      PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);

      //Assert
      Assert.Null(person_response_from_get);
    }


    //If we supply a valid person id, it should return the valid person details as PersonResponse object
    [Fact]
    public void GetPersonByPersonID_WithPersonID()
    {
      //Arange
      CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
      CountryResponse country_response = _countriesService.AddCountry(country_request);

      PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person name...", Email = "email@sample.com", Address = "address", CountryID = country_response.CountryID, DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, ReceiveNewsLetters = false };
      
      PersonResponse person_response_from_add = _personService.AddPerson(person_request);

      PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);

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
      List<PersonResponse> persons_from_get = _personService.GetAllPersons();

      //Assert
      Assert.Empty(persons_from_get);
    }


    //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
    [Fact]
    public void GetAllPersons_AddFewPersons()
    {
      //Arrange
      CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
      CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

      CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
      CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

      PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

      PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

      PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

      List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

      List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

      foreach (PersonAddRequest person_request in person_requests)
      {
        PersonResponse person_response = _personService.AddPerson(person_request);
        person_response_list_from_add.Add(person_response);
      }

      //print person_response_list_from_add
      _testOutputHelper.WriteLine("Expected:");
      foreach (PersonResponse person_response_from_add in person_response_list_from_add)
      {
        _testOutputHelper.WriteLine(person_response_from_add.ToString());
      }

      //Act
      List<PersonResponse> persons_list_from_get = _personService.GetAllPersons();

      //print persons_list_from_get
      _testOutputHelper.WriteLine("Actual:");
      foreach (PersonResponse person_response_from_get in persons_list_from_get)
      {
        _testOutputHelper.WriteLine(person_response_from_get.ToString());
      }

      //Assert
      foreach (PersonResponse person_response_from_add in person_response_list_from_add)
      {
        Assert.Contains(person_response_from_add, persons_list_from_get);
      }
    }
    #endregion


    #region GetFilteredPersons

    //If the search text is empty and search by is "PersonName", it should return all persons
    [Fact]
    public void GetFilteredPersons_EmptySearchText()
    {
      //Arrange
      CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
      CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

      CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
      CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

      PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

      PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

      PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

      List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

      List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

      foreach (PersonAddRequest person_request in person_requests)
      {
        PersonResponse person_response = _personService.AddPerson(person_request);
        person_response_list_from_add.Add(person_response);
      }

      //print person_response_list_from_add
      _testOutputHelper.WriteLine("Expected:");
      foreach (PersonResponse person_response_from_add in person_response_list_from_add)
      {
        _testOutputHelper.WriteLine(person_response_from_add.ToString());
      }

      //Act
      List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

      //print persons_list_from_get
      _testOutputHelper.WriteLine("Actual:");
      foreach (PersonResponse person_response_from_get in persons_list_from_search)
      {
        _testOutputHelper.WriteLine(person_response_from_get.ToString());
      }

      //Assert
      foreach (PersonResponse person_response_from_add in person_response_list_from_add)
      {
        Assert.Contains(person_response_from_add, persons_list_from_search);
      }
    }


    //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
    [Fact]
    public void GetFilteredPersons_SearchByPersonName()
    {
      //Arrange
      CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
      CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

      CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
      CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

      PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

      PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

      PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

      List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

      List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

      foreach (PersonAddRequest person_request in person_requests)
      {
        PersonResponse person_response = _personService.AddPerson(person_request);
        person_response_list_from_add.Add(person_response);
      }

      //print person_response_list_from_add
      _testOutputHelper.WriteLine("Expected:");
      foreach (PersonResponse person_response_from_add in person_response_list_from_add)
      {
        _testOutputHelper.WriteLine(person_response_from_add.ToString());
      }

      //Act
      List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

      //print persons_list_from_get
      _testOutputHelper.WriteLine("Actual:");
      foreach (PersonResponse person_response_from_get in persons_list_from_search)
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
            Assert.Contains(person_response_from_add, persons_list_from_search);
          }
        }
      }
    }

    #endregion


    #region GetSortedPersons

    //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
    [Fact]
    public void GetSortedPersons()
    {
      //Arrange
      CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
      CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

      CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
      CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

      PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

      PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

      PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

      List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

      List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

      foreach (PersonAddRequest person_request in person_requests)
      {
        PersonResponse person_response = _personService.AddPerson(person_request);
        person_response_list_from_add.Add(person_response);
      }

      //print person_response_list_from_add
      _testOutputHelper.WriteLine("Expected:");
      foreach (PersonResponse person_response_from_add in person_response_list_from_add)
      {
        _testOutputHelper.WriteLine(person_response_from_add.ToString());
      }
      List<PersonResponse> allPersons = _personService.GetAllPersons();

      //Act
      List<PersonResponse> persons_list_from_sort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

      //print persons_list_from_get
      _testOutputHelper.WriteLine("Actual:");
      foreach (PersonResponse person_response_from_get in persons_list_from_sort)
      {
        _testOutputHelper.WriteLine(person_response_from_get.ToString());
      }
      person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

      //Assert
      for (int i = 0; i < person_response_list_from_add.Count; i++)
      {
        Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
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
      Assert.Throws<ArgumentNullException>(() => {
        //Act
        _personService.UpdatePerson(person_update_request);
      });
    }


    //When we supply invalid person id, it should throw ArgumentException
    [Fact]
    public void UpdatePerson_InvalidPersonID()
    {
      //Arrange
      PersonUpdateRequest? person_update_request = new PersonUpdateRequest() {  PersonID = Guid.NewGuid() };

      //Assert
      Assert.Throws<ArgumentException>(() => {
        //Act
        _personService.UpdatePerson(person_update_request);
      });
    }


    //When PersonName is null, it should throw ArgumentException
    [Fact]
    public void UpdatePerson_PersonNameIsNull()
    {
      //Arrange
      CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
      CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

      PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryID, Email = "john@example.com", Address = "address...", Gender = GenderOptions.Male };

      PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);

      PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
      person_update_request.PersonName = null;


      //Assert
      Assert.Throws<ArgumentException>(() => {
        //Act
        _personService.UpdatePerson(person_update_request);
      });
      
    }


    //First, add a new person and try to update the person name and email
    [Fact]
    public void UpdatePerson_PersonFullDetailsUpdation()
    {
      //Arrange
      CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
      CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

      PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryID, Address = "Abc road", DateOfBirth = DateTime.Parse("2000-01-01"), Email = "abc@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

      PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);

      PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
      person_update_request.PersonName = "William";
      person_update_request.Email = "william@example.com";

      //Act
      PersonResponse person_response_from_update = _personService.UpdatePerson(person_update_request);

      PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_update.PersonID);

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
      CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };
      CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

      PersonAddRequest person_add_request = new PersonAddRequest() {  PersonName = "Jones", Address = "address", CountryID = country_response_from_add.CountryID, DateOfBirth = Convert.ToDateTime("2010-01-01"), Email = "jones@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

      PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);


      //Act
      bool isDeleted = _personService.DeletePerson(person_response_from_add.PersonID);

      //Assert
      Assert.True(isDeleted);
    }


    //If you supply an invalid PersonID, it should return false
    [Fact]
    public void DeletePerson_InvalidPersonID()
    {
      //Act
      bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

      //Assert
      Assert.False(isDeleted);
    }

    #endregion
  }
}
