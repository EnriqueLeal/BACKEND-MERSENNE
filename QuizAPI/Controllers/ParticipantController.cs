#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;


namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly ILogger<ParticipantController> _logger;
        private readonly IMapper _mapper; // Agrega una referencia a IMapper


        public ParticipantController(QuizDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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
                var participants = await _context.Participants
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToListAsync();

                // Mapear las entidades de los participantes a DTOs
                var participantDTOs = _mapper.Map<IEnumerable<Participant>>(participants);

                // Devolver los DTOs de los participantes
                return Ok(participantDTOs);
            }
            catch (Exception ex)
            {
                // Registrar cualquier error inesperado
                _logger.LogError($"Error al intentar obtener los participantes: {ex.Message}");
                return StatusCode(500, "Se ha producido un error interno al intentar obtener los participantes.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Participant>> GetParticipant(int id)
        {
            try
            {
                // Validar la entrada
                if (id <= 0)
                {
                    return BadRequest("El ID del participante debe ser un número positivo.");
                }

                // Buscar el participante en la base de datos
                var participant = await _context.Participants.FindAsync(id);

                // Verificar si se encontró el participante
                if (participant == null)
                {
                    return NotFound("No se encontró el participante con el ID proporcionado.");
                }

                // Mapear la entidad del participante a un DTO
                var participantDTO = _mapper.Map<Participant>(participant);

                // Registrar la consulta exitosa
                _logger.LogInformation($"Se ha consultado el participante con ID {id}.");

                // Devolver el DTO del participante
                return Ok(participantDTO);
            }
            catch (Exception ex)
            {
                // Registrar cualquier error inesperado
                _logger.LogError($"Error al intentar obtener el participante con ID {id}: {ex.Message}");
                return StatusCode(500, "Se ha producido un error interno al intentar obtener el participante.");
            }
        }

        // PUT: api/Participant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipant(int id, ParticipantRestult _participantResult)
        {
            if (id != _participantResult.ParticipantId)
            {
                return BadRequest();
            }

            // get all current details of the record, then update with quiz results
            Participant participant = _context.Participants.Find(id);
            participant.Score = _participantResult.Score;
            participant.TimeTaken = _participantResult.TimeTaken;

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Participant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Participant>> PostParticipant(Participant participant)
        {
            var temp = _context.Participants
                .Where(x => x.Name == participant.Name
                && x.Email == participant.Email)
                .FirstOrDefault();

            if (temp == null)
            {
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();
            }
            else
                participant = temp;

            return Ok(participant);
        }

        // DELETE: api/Participant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var participant = await _context.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participants.Any(e => e.ParticipantId == id);
        }
    }
}
