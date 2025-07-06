using Bogus;
using PersonTable.Data;
using PersonTable.Data.Entities;
using Person = PersonTable.Data.Entities.Person;

namespace PersonTable.Tools
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Persons.Any())
                return;

            var personFaker = new Faker<Person>()
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.Emails, f =>
                {
                    var emails = new List<Email>();
                    var emailCount = f.Random.Int(1, 3);

                    for (int i = 0; i < emailCount; i++)
                        emails.Add(new Email { Address = f.Internet.Email() });

                    return emails;
                });

            var persons = personFaker.Generate(40);

            context.Persons.AddRange(persons);
            context.SaveChanges();
        }
    }
}
