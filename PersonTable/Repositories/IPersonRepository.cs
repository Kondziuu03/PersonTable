using PersonTable.Data.Entities;

namespace PersonTable.Repositories
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetFilteredAsync(string search, string sortOrder, int page, int pageSize);
        Task<int> CountFilteredAsync(string search);
        Task<Person?> GetByIdAsync(int id);
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int id);
    }
}
