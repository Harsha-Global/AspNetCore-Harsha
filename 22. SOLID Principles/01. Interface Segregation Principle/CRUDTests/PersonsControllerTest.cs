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
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
 public class PersonsControllerTest
 {
  private readonly IPersonsGetterService _personsService;
  private readonly ICountriesService _countriesService;
  private readonly ILogger<PersonsController> _logger;

  private readonly Mock<ICountriesService> _countriesServiceMock;
  private readonly Mock<IPersonsGetterService> _personsServiceMock;
  private readonly Mock<ILogger<PersonsController>> _loggerMock;

  private readonly Fixture _fixture;

  public PersonsControllerTest()
  {
   _fixture = new Fixture();

   _countriesServiceMock = new Mock<ICountriesService>();
   _personsServiceMock = new Mock<IPersonsGetterService>();
   _loggerMock = new Mock<ILogger<PersonsController>>();

   _countriesService = _countriesServiceMock.Object;
   _personsService = _personsServiceMock.Object;
   _logger = _loggerMock.Object;
  }

  #region Index

  [Fact]
  public async Task Index_ShouldReturnIndexViewWithPersonsList()
  {
   //Arrange
   List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

   PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

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

   PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);


   //Act
   IActionResult result = await personsController.Create(person_add_request);

   //Assert
   RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

   redirectResult.ActionName.Should().Be("Index");
  }

  #endregion
 }
}
