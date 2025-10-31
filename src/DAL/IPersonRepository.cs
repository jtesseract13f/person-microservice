namespace PersonMinimalApi.DAL;

public interface IPersonRepository
{
    public Task<Person?> GetPerson(int id);
    public Task<List<Person>> GetPersons();
    public Task<int> CreatePerson(Person person);
    public Task UpdatePerson(Person person);
    
    public Task DeletePerson(Person person);
}