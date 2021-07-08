using System.Collections.Generic;
using FoodyNotes.Entities.Entities;

namespace FoodyNotes.UseCases
{
  public interface IUserService
  {
    User GetById(string id);
    IEnumerable<User> GetAll();
  }
}