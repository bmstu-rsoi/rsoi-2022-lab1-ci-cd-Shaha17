using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonService.API.Context;
using PersonService.API.Models;
using PersonService.API.Models.Entities;

namespace PersonService.API.Repositories;

public class PersonsRepository : IPersonsRepository
{
    private readonly ILogger<PersonsRepository> _logger;
    private readonly PersonsContext _personsContext;

    public PersonsRepository(ILogger<PersonsRepository> logger, PersonsContext personsContext)
    {
        _logger = logger;
        _personsContext = personsContext;
    }

    public async Task<Person> CreateAsync(Person person)
    {
        try
        {
            var entity = new PersonEntity()
            {
                Address = person.Address,
                Age = person.Age,
                Name = person.Name,
                Work = person.Work
            };
            await _personsContext.Persons.AddAsync(entity);
            await _personsContext.SaveChangesAsync();
            var rez = new Person()
            {
                Address = entity.Address,
                Age = entity.Age,
                Id = entity.Id,
                Name = entity.Name,
                Work = entity.Work
            };

            return rez;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while try to create person ({person})", person);
            throw;
        }
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        try
        {
            var personsEntities = await _personsContext.Persons.ToListAsync();
            var persons = personsEntities.Select(entity => new Person()
            {
                Address = entity.Address,
                Age = entity.Age,
                Id = entity.Id,
                Name = entity.Name,
                Work = entity.Work
            });

            return persons;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while try to get persons");
            throw;
        }
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _personsContext.Persons
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            var person = new Person()
            {
                Address = entity.Address,
                Age = entity.Age,
                Id = entity.Id,
                Name = entity.Name,
                Work = entity.Work
            };

            return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while try to get person by id ({id})", id);
            throw;
        }
    }

    public async Task<Person> EditByIdAsync(int id, Person person)
    {
        try
        {
            var entity = await _personsContext.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return null;

            if (person.Address != null)
                entity.Address = person.Address;
            if (person.Age.HasValue) entity.Age = person.Age;
            if (!string.IsNullOrEmpty(person.Name)) entity.Name = person.Name;
            if (!string.IsNullOrEmpty(person.Work)) entity.Work = person.Work;
            await _personsContext.SaveChangesAsync();
            person.Id = entity.Id;
            person.Address = entity.Address;
            person.Age = entity.Age;
            person.Name = entity.Name;
            person.Work = entity.Work;
            return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while try to edit person by id ({id}) ({person})", id, person);
            throw;
        }
    }

    public async Task<Person> DeleteByIdAsync(int id)
    {
        try
        {
            var entity = await _personsContext.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return null;

            _personsContext.Persons.Remove(entity);
            await _personsContext.SaveChangesAsync();

            var person = new Person()
            {
                Address = entity.Address,
                Age = entity.Age,
                Id = entity.Id,
                Name = entity.Name,
                Work = entity.Work
            };

            return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while try to delete person by id ({id})", id);
            throw;
        }
    }
}