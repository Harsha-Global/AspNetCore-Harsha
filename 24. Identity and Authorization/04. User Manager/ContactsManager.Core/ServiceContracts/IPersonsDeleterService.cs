using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
 /// <summary>
 /// Represents business logic (delete) for manipulating Perosn entity
 /// </summary>
 public interface IPersonsDeleterService
 {
  /// <summary>
  /// Deletes a person based on the given person id
  /// </summary>
  /// <param name="PersonID">PersonID to delete</param>
  /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
  Task<bool> DeletePerson(Guid? personID);
 }
}
