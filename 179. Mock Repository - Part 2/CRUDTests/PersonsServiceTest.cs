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
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;

namespace CRUDTests
{
 public class PersonsServiceTest
 {
  //private fields
  private readonly IPersonsService _personService;
  private readonly ICountriesService _countriesService;

  private readonly Mock<IPersonsRepository> _personRepositoryMock;
  private readonly IPersonsRepository _personsRepository;

  private readonly ITestOutputHelper _testOutputHelper;
  private readonly IFixture _fixture;

  //constructor
  public PersonsServiceTest(ITestOutputHelper testOutputHelper)
  {
   _fixture = new Fixture();
   _personRepositoryMock = new Mock<IPersonsRepository>();
   _personsRepository = _personRepositoryMock.Object;

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
   _countriesService = new CountriesService(null);

   _personService = new PersonsService(_personsRepository);

   _testOutputHelper = testOutputHelper;
  }

  #region AddPerson

  //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
  [Fact]
  public async Task AddPerson_NullPerson_ToBeArgumentNullException()
  {
   //Arrange
   PersonAddRequest? personAddRequest = null;

   //Act
   Func<Task> action = async () =>
   {
    await _personService.AddPerson(personAddRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentNullException>();
  }


  //When we supply null value as PersonName, it should throw ArgumentException
  [Fact]
  public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
  {
   //Arrange
   PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.PersonName, null as string)
    .Create();

   Person person = personAddRequest.ToPerson();

   //When PersonsRepository.AddPerson is called, it has to return the same "person" object
   _personRepositoryMock
    .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
    .ReturnsAsync(person);

   //Act
   Func<Task> action = async () =>
   {
    await _personService.AddPerson(personAddRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
  [Fact]
  public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
  {
   //Arrange
   PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
    .With(temp => temp.Email, "someone@example.com")
    .Create();

   Person person = personAddRequest.ToPerson();
   PersonResponse person_response_expected = person.ToPersonResponse();

   //If we supply any argument value to the AddPerson method, it should return the same return value
   _personRepositoryMock.Setup
    (temp => temp.AddPerson(It.IsAny<Person>()))
    .ReturnsAsync(person);

   //Act
   PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);

   person_response_expected.PersonID = person_response_from_add.PersonID;

   //Assert
   person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
   person_response_from_add.Should().Be(person_response_expected);
  }

  #endregion


  #region GetPersonByPersonID

  //If we supply null as PersonID, it should return null as PersonResponse
  [Fact]
  public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
  {
   //Arrange
   Guid? personID = null;

   //Act
   PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personID);

   //Assert
   person_response_from_get.Should().BeNull();
  }


  //If we supply a valid person id, it should return the valid person details as PersonResponse object
  [Fact]
  public async Task GetPersonByPersonID_WithPersonID_ToBeSucessful()
  {
   //Arange
   Person person = _fixture.Build<Person>()
    .With(temp => temp.Email, "email@sample.com")
    .With(temp => temp.Country, null as Country)
    .Create();
   PersonResponse person_response_expected = person.ToPersonResponse();

   _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
    .ReturnsAsync(person);

   //Act
   PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person.PersonID);

   //Assert
   person_response_from_get.Should().Be(person_response_expected);
  }

  #endregion


  #region GetAllPersons

  //The GetAllPersons() should return an empty list by default
  [Fact]
  public async Task GetAllPersons_ToBeEmptyList()
  {
   //Arrange
   var persons = new List<Person>();
   _personRepositoryMock
    .Setup(temp => temp.GetAllPersons())
    .ReturnsAsync(persons);

   //Act
   List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

   //Assert
   persons_from_get.Should().BeEmpty();
  }


  //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
  [Fact]
  public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
  {
   //Arrange
   List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

   List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_expected)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }

   _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

   //Act
   List<PersonResponse> persons_list_from_get = await _personService.GetAllPersons();

   //print persons_list_from_get
   _testOutputHelper.WriteLine("Actual:");
   foreach (PersonResponse person_response_from_get in persons_list_from_get)
   {
    _testOutputHelper.WriteLine(person_response_from_get.ToString());
   }

   //Assert
   persons_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
  }
  #endregion


  #region GetFilteredPersons

  //If the search text is empty and search by is "PersonName", it should return all persons
  [Fact]
  public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
  {
   //Arrange
   List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

   List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_expected)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }

   _personRepositoryMock.Setup(temp => temp
   .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
    .ReturnsAsync(persons);

   //Act
   List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

   //print persons_list_from_get
   _testOutputHelper.WriteLine("Actual:");
   foreach (PersonResponse person_response_from_get in persons_list_from_search)
   {
    _testOutputHelper.WriteLine(person_response_from_get.ToString());
   }

   //Assert
   persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
  }


  //Search based on person name with some search string. It should return the matching persons
  [Fact]
  public async Task GetFilteredPersons_SearchByPersonName_ToBeSuccessful()
  {
   //Arrange
   List<Person> persons = new List<Person>() {
    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_1@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_2@example.com")
    .With(temp => temp.Country, null as Country)
    .Create(),

    _fixture.Build<Person>()
    .With(temp => temp.Email, "someone_3@example.com")
    .With(temp => temp.Country, null as Country)
    .Create()
   };

   List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


   //print person_response_list_from_add
   _testOutputHelper.WriteLine("Expected:");
   foreach (PersonResponse person_response_from_add in person_response_list_expected)
   {
    _testOutputHelper.WriteLine(person_response_from_add.ToString());
   }

   _personRepositoryMock.Setup(temp => temp
   .GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
    .ReturnsAsync(persons);

   //Act
   List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "sa");

   //print persons_list_from_get
   _testOutputHelper.WriteLine("Actual:");
   foreach (PersonResponse person_response_from_get in persons_list_from_search)
   {
    _testOutputHelper.WriteLine(person_response_from_get.ToString());
   }

   //Assert
   persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
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
   //person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

   //Assert
   //persons_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add);

   persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
  }
  #endregion


  #region UpdatePerson

  //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
  [Fact]
  public async Task UpdatePerson_NullPerson()
  {
   //Arrange
   PersonUpdateRequest? person_update_request = null;

   //Act
   Func<Task> action = async () =>
   {
    await _personService.UpdatePerson(person_update_request);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentNullException>();
  }


  //When we supply invalid person id, it should throw ArgumentException
  [Fact]
  public async Task UpdatePerson_InvalidPersonID()
  {
   //Arrange
   PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>()
    .Create();

   //Act
   Func<Task> action = async () =>
   {
    await _personService.UpdatePerson(person_update_request);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
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


   //Act
   var action = async () =>
   {
    await _personService.UpdatePerson(person_update_request);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
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
   person_response_from_update.Should().Be(person_response_from_get);
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
   isDeleted.Should().BeTrue();
  }


  //If you supply an invalid PersonID, it should return false
  [Fact]
  public async Task DeletePerson_InvalidPersonID()
  {
   //Act
   bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

   //Assert
   isDeleted.Should().BeFalse();
  }

  #endregion
 }
}
