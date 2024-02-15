#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using QuizAPI.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly ILogger<ParticipantController> _logger;
        private readonly IMapper _mapper; // Agrega una referencia a IMapper

        public UserController(QuizDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validar la paginación
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("El número de página y el tamaño de la página deben ser números positivos.");
                }

                // Calcular el índice inicial de los elementos a recuperar
                int startIndex = (pageNumber - 1) * pageSize;

                // Obtener los participantes de la base de datos paginados
                var Users = await _context.Users
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToListAsync();

                // Mapear las entidades de los participantes a DTOs
                var UersResponse = _mapper.Map<IEnumerable<Users>>(Users);

                // Devolver los DTOs de los Users
                return Ok(UersResponse);
            }
            catch (Exception ex)
            {
                // Registrar cualquier error inesperado
                _logger.LogError($"Error al intentar obtener los Users: {ex.Message}");
                return StatusCode(500, "Se ha producido un error interno al intentar obtener los Users.");
            }
        }


        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<Users>> PostUser(Users user)
        {
            try
            {
                // Verificar si el usuario ya existe
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existingUser != null)
                {
                    return Conflict("El nombre de usuario ya está en uso.");
                }

                // Agregar el usuario al contexto
                _context.Users.Add(user);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Devolver el usuario recién creado con el código de estado 201 Created
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error y devolver un código de estado 500 Internal Server Error
                return StatusCode(500, $"Se produjo un error al intentar crear el usuario: {ex.Message}");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return user;
        }
    }
}
