using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PersonService.API.Models;

namespace PersonService.API.Repositories;

public interface IPersonsRepository
{
    Task<Person> CreateAsync(Person person);
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<Person> EditByIdAsync(int id, Person person);
    Task<Person> DeleteByIdAsync(int id);
}