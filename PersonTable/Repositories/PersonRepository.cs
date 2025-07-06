using Microsoft.EntityFrameworkCore;
using PersonTable.Data;
using PersonTable.Data.Entities;

namespace PersonTable.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetFilteredAsync(string search, string sortOrder, int page, int pageSize)
        {
            var persons = GetFilteredPersonsQuery(search);

            persons = sortOrder switch
            {
                "FirstName" => persons.OrderBy(p => p.FirstName),
                "FirstName_desc" => persons.OrderByDescending(p => p.FirstName),
                "LastName" => persons.OrderBy(p => p.LastName),
                "LastName_desc" => persons.OrderByDescending(p => p.LastName),
                _ => persons.OrderBy(p => p.Id),
            };

            return await persons.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> CountFilteredAsync(string search) =>
            await GetFilteredPersonsQuery(search).CountAsync();

        public async Task<Person?> GetByIdAsync(int id) =>
            await _context.Persons.Include(p => p.Emails).FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person person)
        {
            if(person == null)
                throw new ArgumentNullException(nameof(person));

            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await GetByIdAsync(id);

            if (person == null)
                throw new Exception($"Could not find Person with id: {id}");

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Person> GetFilteredPersonsQuery(string search)
        {
            var persons = _context.Persons.Include(p => p.Emails).AsQueryable();

            if (string.IsNullOrWhiteSpace(search))
                return persons;

            return persons.Where(p => p.FirstName.Contains(search) || p.LastName.Contains(search));
        }
    }
}
