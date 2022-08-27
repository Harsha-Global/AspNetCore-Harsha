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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using AutoFixture;

namespace CRUDTests
{
 public class PersonsServiceTest
 {
  //private fields
  private readonly IPersonsService _personService;
  private readonly ICountriesService _countriesService;
  private readonly ITestOutputHelper _testOutputHelper;
  private readonly IFixture _fixture;

  //constructor
  public PersonsServiceTest(ITestOutputHelper testOutputHelper)
  {
   _fixture = new Fixture();

   var countriesInitialData = new List<Country>() { };
   var personsInitialData = new List<Person>() { };

   //Craete mock for DbContext
   DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
     new DbContextOptionsBuilder<ApplicationDbContext>().Options
    );

   //Access Mock DbContext object
   ApplicationDbContext dbContext = dbContextMock.Object;

   //Create mocks for DbSets'
   dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
   dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

   //Create services based on mocked DbContext object
   _countriesService = new CountriesService(dbContext);

   _personService = new PersonsService(dbContext, _countriesService);

   _testOutputHelper = testOutputHelper;
  }

  #region AddPerson

  //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
  [Fact]
  public async Task AddPerson_NullPerson()
  {
   //Arrange
   PersonAddRequest? personAddRequest = null;

   //Act
   await Assert.ThrowsAsync<ArgumentNullException>(async () =>
   {
    await _personService.AddPerson(personAddRequest);
   });
  }


  //When we supply null value as PersonName, it should throw ArgumentException
  [Fact]
  public async Task AddPerson_PersonNameIsNull()
  {
   //Arrange
   PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, null as string)
    .Create();

   //Assert
   await Assert.ThrowsAsync<ArgumentException>(async () =>
   {
    //Act
    await _personService.AddPerson(personAddRequest);
   });
  }

  //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
  [Fact]
  public async Task AddPerson_ProperPersonDetails()
  {
   //Arrange
   PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone@example.com")
    .Create();

   //Act
   PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);

   List<PersonResponse> persons_list = await _personService.GetAllPersons();

   //Assert
   Assert.True(person_response_from_add.PersonID != Guid.Empty);

   Assert.Contains(person_response_from_add, persons_list);
  }

  #endregion


  #region GetPersonByPersonID

  //If we supply null as PersonID, it should return null as PersonResponse
  [Fact]
  public async Task GetPersonByPersonID_NullPersonID()
  {
   //Arrange
   Guid? personID = null;

   //Act
   PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personID);

   //Assert
   Assert.Null(person_response_from_get);
  }


  //If we supply a valid person id, it should return the valid person details as PersonResponse object
  [Fact]
  public async Task GetPersonByPersonID_WithPersonID()
  {
   //Arange
   CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response = await _countriesService.AddCountry(country_request);

   PersonAddRequest person_request = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "email@sample.com")
    .Create();

   PersonResponse person_response_from_add = await _personService.AddPerson(person_request);

   PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_add.PersonID);

   //Assert
   Assert.Equal(person_response_from_add, person_response_from_get);
  }

  #endregion


  #region GetAllPersons

  //The GetAllPersons() should return an empty list by default
  [Fact]
  public async Task GetAllPersons_EmptyList()
  {
   //Act
   List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

   //Assert
   Assert.Empty(persons_from_get);
  }


  //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
  [Fact]
  public async Task GetAllPersons_AddFewPersons()
  {
   //Arrange
   CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
   CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
   CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

   PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone_1@example.com")
    .Create();

   PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone_2@example.com")
    .Create();

   PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone_3@example.com")
    .Create();

   List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

   List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

   foreach (PersonAddRequest person_request in person_requests)
   {
    PersonResponse person_response = await _personService.AddPerson(person_request);
    person_response_list_from_add.Add(person_response);
   }

   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_from_add)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }

   //Act
   List<PersonResponse> persons_list_from_get = await _personService.GetAllPersons();

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
  public async Task GetFilteredPersons_EmptySearchText()
  {
   //Arrange
   CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
   CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
   CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

   PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone_1@example.com")
    .Create();

   PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone_2@example.com")
    .Create();

   PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone_3@example.com")
    .Create();

   List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

   List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

   foreach (PersonAddRequest person_request in person_requests)
   {
    PersonResponse person_response = await _personService.AddPerson(person_request);
    person_response_list_from_add.Add(person_response);
   }

   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_from_add)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }

   //Act
   List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

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
  public async Task GetFilteredPersons_SearchByPersonName()
  {
   //Arrange
   CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
   CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
   CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

   PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Rahman")
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.CountryID, country_response_1.CountryID)
    .Create();

   PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "mary")
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.CountryID, country_response_1.CountryID)
    .Create();

   PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "scott")
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.CountryID, country_response_2.CountryID)
    .Create();

   List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

   List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

   foreach (PersonAddRequest person_request in person_requests)
   {
    PersonResponse person_response = await _personService.AddPerson(person_request);
    person_response_list_from_add.Add(person_response);
   }

   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_from_add)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }

   //Act
   List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

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
  public async Task GetSortedPersons()
  {
   //Arrange
   CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
   CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
   CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

   PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Smith")
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.CountryID, country_response_1.CountryID)
    .Create();

   PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Mary")
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.CountryID, country_response_1.CountryID)
    .Create();

   PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Rahman")
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.CountryID, country_response_2.CountryID)
    .Create();


   List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

   List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

   foreach (PersonAddRequest person_request in person_requests)
   {
    PersonResponse person_response = await _personService.AddPerson(person_request);
    person_response_list_from_add.Add(person_response);
   }

   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_from_add)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }
   List<PersonResponse> allPersons = await _personService.GetAllPersons();

   //Act
   List<PersonResponse> persons_list_from_sort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

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
  public async Task UpdatePerson_NullPerson()
  {
   //Arrange
   PersonUpdateRequest? person_update_request = null;

   //Assert
   await Assert.ThrowsAsync<ArgumentNullException>(async () =>
   {
    //Act
    await _personService.UpdatePerson(person_update_request);
   });
  }


  //When we supply invalid person id, it should throw ArgumentException
  [Fact]
  public async Task UpdatePerson_InvalidPersonID()
  {
   //Arrange
   PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>()
    .Create();

   //Assert
   await Assert.ThrowsAsync<ArgumentException>(async () =>
   {
    //Act
    await _personService.UpdatePerson(person_update_request);
   });
  }


  //When PersonName is null, it should throw ArgumentException
  [Fact]
  public async Task UpdatePerson_PersonNameIsNull()
  {
   //Arrange
   CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response = await _countriesService.AddCountry(country_request);

   PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Rahman")
    .With(temp => temp.Email, "someone@example.com")
    .With(temp => temp.CountryID, country_response.CountryID)
    .Create();


   PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

   PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
   person_update_request.PersonName = null;


   //Assert
   await Assert.ThrowsAsync<ArgumentException>(async () =>
   {
    //Act
    await _personService.UpdatePerson(person_update_request);
   });

  }


  //First, add a new person and try to update the person name and email
  [Fact]
  public async Task UpdatePerson_PersonFullDetailsUpdation()
  {
   //Arrange
   CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response = await _countriesService.AddCountry(country_request);

   PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Rahman")
    .With(temp => temp.Email, "someone@example.com")
    .With(temp => temp.CountryID, country_response.CountryID)
    .Create();

   PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

   PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
   person_update_request.PersonName = "William";
   person_update_request.Email = "william@example.com";

   //Act
   PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);

   PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_update.PersonID);

   //Assert
   Assert.Equal(person_response_from_get, person_response_from_update);

  }

  #endregion


  #region DeletePerson

  //If you supply an valid PersonID, it should return true
  [Fact]
  public async Task DeletePerson_ValidPersonID()
  {
   //Arrange
   CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

   CountryResponse country_response = await _countriesService.AddCountry(country_request);

   PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, "Rahman")
    .With(temp => temp.Email, "someone@example.com")
    .With(temp => temp.CountryID, country_response.CountryID)
    .Create();

   PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);


   //Act
   bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);

   //Assert
   Assert.True(isDeleted);
  }


  //If you supply an invalid PersonID, it should return false
  [Fact]
  public async Task DeletePerson_InvalidPersonID()
  {
   //Act
   bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

   //Assert
   Assert.False(isDeleted);
  }

  #endregion
 }
}
