using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;

namespace Services
{
  public class PersonsService : IPersonsService
  {
    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
      throw new NotImplementedException();
    }

    public List<PersonResponse> GetAllPersons()
    {
      throw new NotImplementedException();
    }
  }
}
