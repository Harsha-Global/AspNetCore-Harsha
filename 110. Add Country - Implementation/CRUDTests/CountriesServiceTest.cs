using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Xunit;

namespace CRUDTests
{
  public class CountriesServiceTest
  {
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
      _countriesService = new CountriesService();
    }

    //When CountryAddRequest is null, it should throw ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
      //Arrange
      CountryAddRequest? request = null;

      //Assert
      Assert.Throws<ArgumentNullException>(() =>
      {
        //Act
        _countriesService.AddCountry(request);
      });
    }

    //When the CountryName is null, it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
      //Arrange
      CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

      //Assert
      Assert.Throws<ArgumentException>(() =>
      {
        //Act
        _countriesService.AddCountry(request);
      });
    }


    //When the CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
      //Arrange
      CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
      CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

      //Assert
      Assert.Throws<ArgumentException>(() =>
      {
        //Act
        _countriesService.AddCountry(request1);
        _countriesService.AddCountry(request2);
      });
    }


    //When you supply proper country name, it should insert (add) the country to the existing list of countries
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
      //Arrange
      CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

      //Act
      CountryResponse response = _countriesService.AddCountry(request);

      //Assert
      Assert.True(response.CountryID != Guid.Empty);
    }
  }
}
