using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonService.API.Models;
using PersonService.API.Models.Dto;
using PersonService.API.Repositories;

namespace PersonService.API.Controllers
{
    [ApiController]
    [Route("api/v1/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly ILogger<PersonsController> _logger;
        private readonly IPersonsRepository _personsRepository;

        public PersonsController(ILogger<PersonsController> logger, IPersonsRepository personsRepository)
        {
            _logger = logger;
            _personsRepository = personsRepository;
        }

        /// <summary>
        /// Get all persons
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Person>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            try
            {
                var persons = await _personsRepository.GetAllAsync();
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get person by id
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Person>> Get([FromRoute] int id)
        {
            try
            {
                var person = await _personsRepository.GetByIdAsync(id);
                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create new person
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Person), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Person>> Create([FromBody] PersonDto personDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var personToCreate = new Person()
                {
                    Address = personDto.Address,
                    Age = personDto.Age,
                    Name = personDto.Name,
                    Work = personDto.Work
                };

                var person = await _personsRepository.CreateAsync(personToCreate);
                return Created($"api/v1/persons/{person.Id}", person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update person by id
        /// </summary>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Person>> Put([FromRoute] int id, [FromBody] PersonDto personDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var personToUpdate = new Person()
                {
                    Address = personDto.Address,
                    Age = personDto.Age,
                    Name = personDto.Name,
                    Work = personDto.Work
                };

                var person = await _personsRepository.EditByIdAsync(id, personToUpdate);
                if (person == null)
                    return NotFound();
                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete person by id
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var person = await _personsRepository.DeleteByIdAsync(id);
                if (person == null)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}