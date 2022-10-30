using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
 public class CountriesUploaderService : ICountriesUploaderService
 {
  //private field
  private readonly ICountriesRepository _countriesRepository;

  //constructor
  public CountriesUploaderService(ICountriesRepository countriesRepository)
  {
   _countriesRepository = countriesRepository;
  }

  public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
  {
   MemoryStream memoryStream = new MemoryStream();
   await formFile.CopyToAsync(memoryStream);
   int countriesInserted = 0;

   using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

    int rowCount = workSheet.Dimension.Rows;

    for (int row = 2; row <= rowCount; row++)
    {
     string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

     if (!string.IsNullOrEmpty(cellValue))
     {
      string? countryName = cellValue;

      if (await _countriesRepository.GetCountryByCountryName(countryName) == null)
      {
       Country country = new Country() { CountryName = countryName };
       await _countriesRepository.AddCountry(country);

       countriesInserted++;
      }
     }
    }
   }

   return countriesInserted;
  }
 }
}

