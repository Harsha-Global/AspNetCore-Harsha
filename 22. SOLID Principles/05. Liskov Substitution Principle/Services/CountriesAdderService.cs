using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
 public class CountriesAdderService : ICountriesAdderService
 {
  //private field
  private readonly ICountriesRepository _countriesRepository;

  //constructor
  public CountriesAdderService(ICountriesRepository countriesRepository)
  {
   _countriesRepository = countriesRepository;
  }

  public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
  {
   //Validation: countryAddRequest parameter can't be null
   if (countryAddRequest == null)
   {
    throw new ArgumentNullException(nameof(countryAddRequest));
   }

   //Validation: CountryName can't be null
   if (countryAddRequest.CountryName == null)
   {
    throw new ArgumentException(nameof(countryAddRequest.CountryName));
   }

   //Validation: CountryName can't be duplicate
   if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
   {
    throw new ArgumentException("Given country name already exists");
   }

   //Convert object from CountryAddRequest to Country type
   Country country = countryAddRequest.ToCountry();

   //generate CountryID
   country.CountryID = Guid.NewGuid();

   //Add country object into _countries
   await _countriesRepository.AddCountry(country);

   return country.ToCountryResponse();
  }
 }
}

