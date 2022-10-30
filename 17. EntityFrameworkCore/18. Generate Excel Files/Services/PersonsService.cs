using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services.Helpers;
using ServiceContracts.Enums;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace Services
{
  public class PersonsService : IPersonsService
  {
    //private field
    private readonly PersonsDbContext _db;
    private readonly ICountriesService _countriesService;

    //constructor
    public PersonsService(PersonsDbContext personDbContext, ICountriesService countriesService)
    {
      _db = personDbContext;
      _countriesService = countriesService;
    }
    

    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
    {
      //check if PersonAddRequest is not null
      if (personAddRequest == null)
      {
        throw new ArgumentNullException(nameof(personAddRequest));
      }

      //Model validation
      ValidationHelper.ModelValidation(personAddRequest);

      //convert personAddRequest into Person type
      Person person = personAddRequest.ToPerson();

      //generate PersonID
      person.PersonID = Guid.NewGuid();

      //add person object to persons list
      _db.Persons.Add(person);
      await _db.SaveChangesAsync();
      //_db.sp_InsertPerson(person);

      //convert the Person object into PersonResponse type
      return person.ToPersonResponse();
    }


    public async Task<List<PersonResponse>> GetAllPersons()
    {
      //SELECT * from Persons
      var persons = await _db.Persons.Include("Country").ToListAsync();

      return persons
        .Select(temp => temp.ToPersonResponse()).ToList();

      //return _db.sp_GetAllPersons()
      //  .Select(temp => temp.ToPersonResponse()).ToList();
    }


    public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
    {
      if (personID == null)
        return null;

      Person? person = await _db.Persons.Include("Country")
        .FirstOrDefaultAsync(temp => temp.PersonID == personID);
      
      if (person == null)
        return null;

      return person.ToPersonResponse();
    }


    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
    {
      List<PersonResponse> allPersons = await GetAllPersons();
      List<PersonResponse> matchingPersons = allPersons;

      if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
        return matchingPersons;


      switch (searchBy)
      {
        case nameof(PersonResponse.PersonName):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.PersonName) ?
          temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        case nameof(PersonResponse.Email):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Email) ?
          temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;


        case nameof(PersonResponse.DateOfBirth):
          matchingPersons = allPersons.Where(temp =>
          (temp.DateOfBirth != null) ?
          temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
          break;

        case nameof(PersonResponse.Gender):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Gender) ?
          temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        case nameof(PersonResponse.CountryID):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Country) ?
          temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        case nameof(PersonResponse.Address):
          matchingPersons = allPersons.Where(temp =>
          (!string.IsNullOrEmpty(temp.Address) ?
          temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
          break;

        default: matchingPersons = allPersons; break;
      }
      return matchingPersons;
    }


    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
      if (string.IsNullOrEmpty(sortBy))
        return allPersons;

      List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
      {
        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

        (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

        (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

        (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

        _ => allPersons
      };

      return sortedPersons;
    }


    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
      if (personUpdateRequest == null)
        throw new ArgumentNullException(nameof(Person));

      //validation
      ValidationHelper.ModelValidation(personUpdateRequest);

      //get matching person object to update
      Person? matchingPerson = await _db.Persons
        .FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID);
      if (matchingPerson == null)
      {
        throw new ArgumentException("Given person id doesn't exist");
      }

      //update all details
      matchingPerson.PersonName = personUpdateRequest.PersonName;
      matchingPerson.Email = personUpdateRequest.Email;
      matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
      matchingPerson.Gender = personUpdateRequest.Gender.ToString();
      matchingPerson.CountryID = personUpdateRequest.CountryID;
      matchingPerson.Address = personUpdateRequest.Address;
      matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

      await _db.SaveChangesAsync(); //UPDATE

      return matchingPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personID)
    {
      if (personID == null)
      {
        throw new ArgumentNullException(nameof(personID));
      }

      Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personID);
      if (person == null)
        return false;

      _db.Persons.Remove(
        _db.Persons.First(temp => temp.PersonID == personID));
      await _db.SaveChangesAsync();

      return true;
    }

    public async Task<MemoryStream> GetPersonsCSV()
    {
      MemoryStream memoryStream = new MemoryStream();
      StreamWriter streamWriter = new StreamWriter(memoryStream);

      CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
      CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

      //PersonName,Email,DateOfBirth,Age,Gender,Country,Address,ReceiveNewsLetters
      csvWriter.WriteField(nameof(PersonResponse.PersonName));
      csvWriter.WriteField(nameof(PersonResponse.Email));
      csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
      csvWriter.WriteField(nameof(PersonResponse.Age));
      csvWriter.WriteField(nameof(PersonResponse.Country));
      csvWriter.WriteField(nameof(PersonResponse.Address));
      csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
      csvWriter.NextRecord();

      List<PersonResponse> persons = await _db.Persons
        .Include("Country")
        .Select(temp => temp.ToPersonResponse()).ToListAsync();

      foreach (PersonResponse person in persons)
      {
        csvWriter.WriteField(person.PersonName);
        csvWriter.WriteField(person.Email);
        if (person.DateOfBirth.HasValue)
          csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
        else
          csvWriter.WriteField("");
        csvWriter.WriteField(person.Age);
        csvWriter.WriteField(person.Country);
        csvWriter.WriteField(person.Address);
        csvWriter.WriteField(person.ReceiveNewsLetters);
        csvWriter.NextRecord();
        csvWriter.Flush();
      }

      memoryStream.Position = 0;
      return memoryStream;
    }

    public async Task<MemoryStream> GetPersonsExcel()
    {
      MemoryStream memoryStream = new MemoryStream();
      using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
      {
        ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
        workSheet.Cells["A1"].Value = "Person Name";
        workSheet.Cells["B1"].Value = "Email";
        workSheet.Cells["C1"].Value = "Date of Birth";
        workSheet.Cells["D1"].Value = "Age";
        workSheet.Cells["E1"].Value = "Gender";
        workSheet.Cells["F1"].Value = "Country";
        workSheet.Cells["G1"].Value = "Address";
        workSheet.Cells["H1"].Value = "Receive News Letters";

        using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
        {
          headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
          headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
          headerCells.Style.Font.Bold = true;
        }

        int row = 2;
        List<PersonResponse> persons = _db.Persons
          .Include("Country").Select(temp => temp.ToPersonResponse())
          .ToList();
        foreach (PersonResponse person in persons)
        {
          workSheet.Cells[row, 1].Value = person.PersonName;
          workSheet.Cells[row, 2].Value = person.Email;
          if (person.DateOfBirth.HasValue)
            workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
          workSheet.Cells[row, 4].Value = person.Age;
          workSheet.Cells[row, 5].Value = person.Gender;
          workSheet.Cells[row, 6].Value = person.Country;
          workSheet.Cells[row, 7].Value = person.Address;
          workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

          row++;
        }

        workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

        await excelPackage.SaveAsync();
      }

      memoryStream.Position = 0;
      return memoryStream;
    }
  }
}
