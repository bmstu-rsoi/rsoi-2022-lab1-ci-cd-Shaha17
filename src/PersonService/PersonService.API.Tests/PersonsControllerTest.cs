using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.API.Context;
using PersonService.API.Controllers;
using PersonService.API.Models;
using PersonService.API.Models.Dto;
using PersonService.API.Repositories;

namespace PersonService.API.Tests;

public class PersonsControllerTest
{
    private Mock<ILogger<PersonsController>> _logger;
    private IPersonsRepository _personsRepository;

    public PersonsControllerTest()
    {
        _logger = new Mock<ILogger<PersonsController>>();
        var opt = new DbContextOptionsBuilder<PersonsContext>()
            .UseInMemoryDatabase("PersonsDB").Options;
        var context = new PersonsContext(opt);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        _personsRepository =
            new PersonsRepository(new Logger<PersonsRepository>(new LoggerFactory()), context);
    }

    [Fact]
    public async Task GetByIdTest()
    {
        var controller = new PersonsController(_logger.Object, _personsRepository);

        var data = new PersonDto() {Age = 22, Name = "Shahzod"};
        var res = await controller.Create(data);
        var createdPerson = (Person) ((CreatedResult) res.Result).Value;

        var result = await controller.Get(createdPerson.Id);

        Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(createdPerson.Id, ((Person) ((CreatedResult) res.Result).Value).Id);
    }

    [Fact]
    public async Task CreateTest()
    {
        var controller = new PersonsController(_logger.Object, _personsRepository);

        var data = new PersonDto() {Age = 22, Name = "Shahzod"};
        var result = await controller.Create(data);

        Assert.IsType<CreatedResult>(result.Result);
    }

    [Fact]
    public async Task UpdateTest()
    {
        var controller = new PersonsController(_logger.Object, _personsRepository);

        var data = new PersonDto() {Age = 22, Name = "Shahzod"};
        var result = await controller.Put(0, data);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void DeleteTest()
    {
        var controller = new PersonsController(_logger.Object, _personsRepository);

        var result = controller.Delete(0);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}