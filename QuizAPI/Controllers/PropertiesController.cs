using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using API.DataAccess.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertiesController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        // GET: api/Properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Properties>>> GetProperties()
        {
            var properties = await _propertyRepository.GetPropertiesAsync();
            return Ok(properties);
        }

        // GET: api/Properties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Properties>> GetProperty(int id)
        {
            var property = await _propertyRepository.GetPropertyAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            return property;
        }

        // POST: api/Properties
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Properties>> PostProperty(Properties property)
        {
            await _propertyRepository.CreatePropertyAsync(property);
            return CreatedAtAction(nameof(GetProperty), new { id = property.PropertyID }, property);
        }

        // PUT: api/Properties/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProperty(int id, Properties property)
        {
            try
            {
                await _propertyRepository.UpdatePropertyAsync(id, property);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("ID mismatch");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Property not found");
            }
        }

        // DELETE: api/Properties/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                await _propertyRepository.DeletePropertyAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Property not found");
            }
        }
    }

}
