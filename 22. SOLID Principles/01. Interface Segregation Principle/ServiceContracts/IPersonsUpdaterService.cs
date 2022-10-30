using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
 /// <summary>
 /// Represents business logic for manipulating Perosn entity
 /// </summary>
 public interface IPersonsUpdaterService
 {
  /// <summary>
  /// Updates the specified person details based on the given person ID
  /// </summary>
  /// <param name="personUpdateRequest">Person details to update, including person id</param>
  /// <returns>Returns the person response object after updation</returns>
  Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
 }
}
