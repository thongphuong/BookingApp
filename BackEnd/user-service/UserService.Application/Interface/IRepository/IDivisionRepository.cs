using UserService.Domain;
using UserService.Service.DTO;

namespace UserService.Service.Interface
{
    public interface IDivisionRepository : IGenericRepository<Divisions>
    {
        public Task<List<SelectResponseDTO>> SelectDivision(string query, Guid department);

    }
}
