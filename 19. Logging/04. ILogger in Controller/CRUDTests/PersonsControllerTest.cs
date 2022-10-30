using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using CRUDExample.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CRUDTests
{
 public class PersonsControllerTest
 {
  private readonly IPersonsService _personsService;
  private readonly ICountriesService _countriesService;

  private readonly Mock<ICountriesService> _countriesServiceMock;
  private readonly Mock<IPersonsService> _personsServiceMock;

  private readonly Fixture _fixture;

  public PersonsControllerTest()
  {
   _fixture = new Fixture();

   _countriesServiceMock = new Mock<ICountriesService>();
   _personsServiceMock = new Mock<IPersonsService>();

   _countriesService = _countriesServiceMock.Object;
   _personsService = _personsServiceMock.Object;
  }

  #region Index

  [Fact]
  public async Task Index_ShouldReturnIndexViewWithPersonsList()
  {
   //Arrange
   List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

   PersonsController personsController = new PersonsController(_personsService, _countriesService);

   _personsServiceMock
    .Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
    .ReturnsAsync(persons_response_list);

   _personsServiceMock
    .Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
    .ReturnsAsync(persons_response_list);

   //Act
   IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

   //Assert
   ViewResult viewResult = Assert.IsType<ViewResult>(result);

   viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
   viewResult.ViewData.Model.Should().Be(persons_response_list);
  }
  #endregion


  #region Create

  [Fact]
  public async void Create_IfModelErrors_ToReturnCreateView()
  {
   //Arrange
   PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

   PersonResponse person_response = _fixture.Create<PersonResponse>();

   List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

   _countriesServiceMock
    .Setup(temp => temp.GetAllCountries())
    .ReturnsAsync(countries);

   _personsServiceMock
    .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
    .ReturnsAsync(person_response);

   PersonsController personsController = new PersonsController(_personsService, _countriesService);


   //Act
   personsController.ModelState.AddModelError("PersonName", "Person Name can't be blank");

   IActionResult result = await personsController.Create(person_add_request);

   //Assert
   ViewResult viewResult = Assert.IsType<ViewResult>(result);

   viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();

   viewResult.ViewData.Model.Should().Be(person_add_request);
  }


  [Fact]
  public async void Create_IfNoModelErrors_ToReturnRedirectToIndex()
  {
   //Arrange
   PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

   PersonResponse person_response = _fixture.Create<PersonResponse>();

   List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

   _countriesServiceMock
    .Setup(temp => temp.GetAllCountries())
    .ReturnsAsync(countries);

   _personsServiceMock
    .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
    .ReturnsAsync(person_response);

   PersonsController personsController = new PersonsController(_personsService, _countriesService);


   //Act
   IActionResult result = await personsController.Create(person_add_request);

   //Assert
   RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

   redirectResult.ActionName.Should().Be("Index");
  }

  #endregion
 }
}
