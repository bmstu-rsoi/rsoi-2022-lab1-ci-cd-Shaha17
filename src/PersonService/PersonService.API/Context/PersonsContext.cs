using Microsoft.EntityFrameworkCore;
using PersonService.API.Models.Entities;

namespace PersonService.API.Context;

public class PersonsContext : DbContext
{
    /// <inheritdoc />
    public PersonsContext()
    {
    }

    public PersonsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PersonEntity> Persons { get; set; }
}