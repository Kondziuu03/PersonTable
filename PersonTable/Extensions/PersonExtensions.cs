using PersonTable.Data.Entities;
using PersonTable.Models;

namespace PersonTable.Extensions
{
    public static class PersonExtensions
    {
        public static Person MapModelToObject(this PersonModel model)
            => new Person
            {
                Id = model.Id ?? 0,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Emails = model.Emails.Select(e => new Email { Address = e.Address }).ToList(),
                Description = model.Description
            };

        public static PersonModel MapObjectToModel(this Person person)
            => new PersonModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Emails = person.Emails.Select(e => new EmailModel { Address = e.Address }).ToList(),
                Description = person.Description
            };
    }
}
