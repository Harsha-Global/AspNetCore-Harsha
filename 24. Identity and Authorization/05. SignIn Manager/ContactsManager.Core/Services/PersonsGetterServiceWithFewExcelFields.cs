using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
 public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
 {
  private readonly PersonsGetterService _personGetterService;

  public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
  {
   _personGetterService = personsGetterService;
  }

  public async Task<List<PersonResponse>> GetAllPersons()
  {
   return await _personGetterService.GetAllPersons();
  }

  public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
  {
   return await _personGetterService.GetFilteredPersons(searchBy, searchString);
  }

  public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
  {
   return await _personGetterService.GetPersonByPersonID(personID);
  }

  public async Task<MemoryStream> GetPersonsCSV()
  {
   return await _personGetterService.GetPersonsCSV();
  }

  public async Task<MemoryStream> GetPersonsExcel()
  {
   MemoryStream memoryStream = new MemoryStream();
   using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
    workSheet.Cells["A1"].Value = "Person Name";
    workSheet.Cells["B1"].Value = "Age";
    workSheet.Cells["C1"].Value = "Gender";

    using (ExcelRange headerCells = workSheet.Cells["A1:C1"])
    {
     headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
     headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
     headerCells.Style.Font.Bold = true;
    }

    int row = 2;
    List<PersonResponse> persons = await GetAllPersons();

    foreach (PersonResponse person in persons)
    {
     workSheet.Cells[row, 1].Value = person.PersonName;
     workSheet.Cells[row, 2].Value = person.Age;
     workSheet.Cells[row, 3].Value = person.Gender;

     row++;
    }

    workSheet.Cells[$"A1:C{row}"].AutoFitColumns();

    await excelPackage.SaveAsync();
   }

   memoryStream.Position = 0;
   return memoryStream;
  }
 }
}
