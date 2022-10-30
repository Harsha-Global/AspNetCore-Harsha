using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using FluentAssertions;

namespace CRUDTests
{
 public class CountriesServiceTest
 {
  private readonly ICountriesService _countriesService;
  private readonly IFixture _fixture;

  //constructor
  public CountriesServiceTest()
  {
   _fixture = new Fixture();

   var countriesInitialData = new List<Country>() { };

   DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
     new DbContextOptionsBuilder<ApplicationDbContext>().Options
    );

   ApplicationDbContext dbContext = dbContextMock.Object;
   dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

   _countriesService = new CountriesService(dbContext);
  }


  #region AddCountry

  //When CountryAddRequest is null, it should throw ArgumentNullException
  [Fact]
  public async Task AddCountry_NullCountry()
  {
   //Arrange
   CountryAddRequest? request = null;

   //Act
   var action = async () =>
   {
    await _countriesService.AddCountry(request);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentNullException>();
  }


  //When the CountryName is null, it should throw ArgumentException
  [Fact]
  public async Task AddCountry_CountryNameIsNull()
  {
   //Arrange
   CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
    .With(temp => temp.CountryName, null as string)
    .Create();

   //Act
   var action = async () =>
   {
    await _countriesService.AddCountry(request);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumenNullException>();
  }


  //When the CountryName is duplicate, it should throw ArgumentException
  [Fact]
  public async Task AddCountry_DuplicateCountryName()
  {
   //Arrange
   CountryAddRequest? request1 = _fixture.Build<CountryAddRequest>()
    .Create();
   CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
    .With(temp => temp.CountryName, request1.CountryName)
    .Create();

   //Act
   var action = async () =>
   {
    await _countriesService.AddCountry(request1);
    await _countriesService.AddCountry(request2);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  //When you supply proper country name, it should insert (add) the country to the existing list of countries
  [Fact]
  public async Task AddCountry_ProperCountryDetails()
  {
   //Arrange
   CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
    .Create();

   //Act
   CountryResponse response = await _countriesService.AddCountry(request);
   List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

   //Assert
   response.CountryID.Should().NotBe(Guid.Empty);
   countries_from_GetAllCountries.Should().Contain(response);
  }

  #endregion


  #region GetAllCountries

  [Fact]
  //The list of countries should be empty by default (before adding any countries)
  public async Task GetAllCountries_EmptyList()
  {
   //Act
   List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

   //Assert
   actual_country_response_list.Should().BeEmpty();
  }


  [Fact]
  public async Task GetAllCountries_AddFewCountries()
  {
   //Arrange
   List<CountryAddRequest> country_request_list = new List<CountryAddRequest>() {
      _fixture.Build<CountryAddRequest>()
       .Create(),
      _fixture.Build<CountryAddRequest>()
       .Create()
      };

   //Act
   List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

   foreach (CountryAddRequest country_request in country_request_list)
   {
    countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
   }

   List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

   //Assert
   actualCountryResponseList.Should().BeEquivalentTo(countries_list_from_add_country);
  }
  #endregion


  #region GetCountryByCountryID

  [Fact]
  //If we supply null as CountryID, it should return null as CountryResponse
  public async Task GetCountryByCountryID_NullCountryID()
  {
   //Arrange
   Guid? countrID = null;

   //Act
   CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countrID);


   //Assert
   country_response_from_get_method.Should().BeNull();
  }


  [Fact]
  //If we supply a valid country id, it should return the matching country details as CountryResponse object
  public async Task GetCountryByCountryID_ValidCountryID()
  {
   //Arrange
   CountryAddRequest? country_add_request = _fixture.Build<CountryAddRequest>()
    .Create();

   CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

   //Act
   CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

   //Assert
   country_response_from_get.Should().Be(country_response_from_add);
  }
  #endregion
 }
}
