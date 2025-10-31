using Microsoft.EntityFrameworkCore;
using PersonMinimalApi.DAL;

namespace PersonMinimalApi;

public class PersonRepository(AppDbContext context) : IPersonRepository
{
    public async Task<Person?> GetPerson(int id)
    {
        return await context.Persons.FindAsync(id);
    }

    public async Task<List<Person>> GetPersons()
    {
        return await context.Persons.ToListAsync();
    }

    public async Task<int> CreatePerson(Person person)
    {
        var newPerson = await context.Persons.AddAsync(person);
        await context.SaveChangesAsync();
        return newPerson.Entity.Id;
    }

    public async Task UpdatePerson(Person person)
    {
        context.Update(person);
        await context.SaveChangesAsync();
    }

    public async Task DeletePerson(Person person)
    {
        context.Remove(person);
        await context.SaveChangesAsync();
    }
}