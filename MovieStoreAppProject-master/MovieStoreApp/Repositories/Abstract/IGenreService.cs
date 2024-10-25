using MovieStoreApp.Models.Domain;
using MovieStoreApp.Models.DTO;
namespace MovieStoreApp.Repositories.Abstract
{
    public interface IGenreService
    {
       bool Add(Genre model);
       bool Update(Genre model);
       Genre GetById(int id);
       bool Delete(int id);
      List<Genre> List();

    }
}
