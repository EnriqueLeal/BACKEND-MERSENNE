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
using API.DataAccess.Interfaces;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userRepository.GetUsersAsync(pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Manejar errores
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Users
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Users>> PostUser(Users user)
        {
            try
            {
                await _userRepository.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
            }
            catch (Exception ex)
            {
                // Manejar errores
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(id);
                if (user == null)
                {
                    return NotFound("Usuario no encontrado.");
                }
                return user;
            }
            catch (Exception ex)
            {
                // Manejar errores
                return BadRequest(ex.Message);
            }
        }
    }

}
