using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Repositories
{
 public class PersonsRepository : IPersonsRepository
 {
  private readonly ApplicationDbContext _db;
  private readonly ILogger<PersonsRepository> _logger;

  public PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
  {
   _db = db;
   _logger = logger;
  }

  public async Task<Person> AddPerson(Person person)
  {
   _db.Persons.Add(person);
   await _db.SaveChangesAsync();

   return person;
  }

  public async Task<bool> DeletePersonByPersonID(Guid personID)
  {
   _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonID == personID));
   int rowsDeleted = await _db.SaveChangesAsync();

   return rowsDeleted > 0;
  }

  public async Task<List<Person>> GetAllPersons()
  {
   _logger.LogInformation("GetAllPersons of PersonsRepository");

   return await _db.Persons.Include("Country").ToListAsync();
  }

  public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
  {
   _logger.LogInformation("GetFilteredPersons of PersonsRepository");

   return await _db.Persons.Include("Country")
    .Where(predicate)
    .ToListAsync();
  }

  public async Task<Person?> GetPersonByPersonID(Guid personID)
  {
   return await _db.Persons.Include("Country")
    .FirstOrDefaultAsync(temp => temp.PersonID == personID);
  }

  public async Task<Person> UpdatePerson(Person person)
  {
   Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);

   if (matchingPerson == null)
    return person;

   matchingPerson.PersonName = person.PersonName;
   matchingPerson.Email = person.Email;
   matchingPerson.DateOfBirth = person.DateOfBirth;
   matchingPerson.Gender = person.Gender;
   matchingPerson.CountryID = person.CountryID;
   matchingPerson.Address = person.Address;
   matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

   int countUpdated = await _db.SaveChangesAsync();

   return matchingPerson;
  }
 }
}
