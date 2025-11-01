using Microsoft.EntityFrameworkCore;
using PersonMinimalApi;

namespace Tests;

public class PersonRepositoryTest
{
    readonly AppDbContext _context;
    PersonRepository _personRepository;
    public PersonRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _personRepository = new PersonRepository(_context);
    }
    
    [Fact]
    public async Task TestIsCreating()
    {
        var entityId = await _personRepository.CreatePerson(new Person()
        {
            Name = "John Doe",
            Age = 20
        });
        var entity = await _context.Persons.FirstOrDefaultAsync(p => entityId == p.Id);
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task TestIsDeleting()
    {
        var entityId = await _personRepository.CreatePerson(new Person()
        {
            Name = "John Doe for Delete",
        });
        var entity = await _context.Persons.FirstOrDefaultAsync(p => entityId == p.Id);
        await _personRepository.DeletePerson(entity);
        
        Assert.Null(_context.Persons.FirstOrDefault(p => p.Id == entityId));
    }

    [Fact]
    public async Task TestIsUpdating()
    {
        var entityId = await _personRepository.CreatePerson(new Person()
        {
            Name = "John Doe for Update",
            Age = 20
        });
        var entity = await _personRepository.GetPerson(entityId);
        
        entity.Name = "John Doe Updated";
        entity.Address = "BMSTU";
        await _personRepository.UpdatePerson(entity);
        
        Assert.NotEqual("John Doe for Update", entity.Name);
        Assert.Equal("BMSTU", entity.Address);
    }
}