using PersonMinimalApi.DAL;
using PersonMinimalApi.DTO;

namespace PersonMinimalApi;

public class PersonService(IPersonRepository personRepository)
{
    public async Task<List<PersonResponse>> GetAll()
    {
        return (await personRepository.GetPersons())
            .Select(x => new PersonResponse(x.Id, x.Name, x.Age, x.Address, x.Work))
            .ToList();
    }

    public async Task<PersonResponse?> GetById(int id)
    {
        var entity = await personRepository.GetPerson(id);
        return entity == null ? 
            null : 
            new  PersonResponse(entity.Id, entity.Name, entity.Age, entity.Address, entity.Work);
    }

    public async Task<int> Create(PersonRequest personRequest)
    {
        return await personRepository.CreatePerson(new Person()
        {
            Name = personRequest.Name,
            Age = personRequest.Age,
            Address = personRequest.Address,
            Work = personRequest.Work
        });
    }

    public async Task<int?> Update(int id, PersonPatchRequest personRequest)
    {
        var entity = await personRepository.GetPerson(id);
        if (entity == null) return null;
        entity.Name = personRequest.Name ?? entity.Name;
        entity.Age = personRequest.Age ?? entity.Age;
        entity.Address = personRequest.Address ?? entity.Address;
        entity.Work = personRequest.Work ?? entity.Work;
        
        await personRepository.UpdatePerson(entity);
        return entity.Id;
    }

    public async Task<int?> Delete(int id)
    {
        var entity = await personRepository.GetPerson(id);
        if (entity == null) return null;
        await personRepository.DeletePerson(entity);
        return id;
    }
}