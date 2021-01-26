using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WizardioApi.Dto;
using WizardioApi.Models;

namespace WizardioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSessionsController : ControllerBase
    {
        private readonly WizardioContext _context;

        public GameSessionsController(WizardioContext context)
        {
            _context = context;
        }

        // GET: api/GameSessions
        [HttpGet]
        public async Task<ActionResult<Response<List<GameSessions>>>> GetGameSessions()
        {
            var sessions = await _context.GameSessions.ToListAsync();
            Response<List<GameSessions>> response = new Response<List<GameSessions>>()
            {
                Entity = sessions,
                HasErrors = false
            };
            if (sessions == null)
            {
                response.HasErrors = true;
                response.Message = "Could not obtain game sessions from the database";
            }
            return response;
        }

        // GET: api/GameSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GameSessions>>> GetGameSession(int id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);
            Response<GameSessions> response = new Response<GameSessions>()
            {
                Entity = gameSession,
                HasErrors = false
            };
            if (gameSession == null)
            {
                response.HasErrors = true;
                response.Message = String.Format("Could not obtain game sessions from the database with id {0}", id);
            }
            return response;
        }

        // PUT: api/GameSessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameSession(int id, GameSessions gameSession)
        {
            if (id != gameSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameSessionExists(id))
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

        // POST: api/GameSessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Response<GameSessions>>> PostGameSession(GameSessions gameSession)
        {
            _context.GameSessions.Add(gameSession);
            await _context.SaveChangesAsync();
            Response<GameSessions> response = new Response<GameSessions>()
            {
                Entity = gameSession,
                HasErrors = false
            };
            return CreatedAtAction("GetGameSession", new { id = gameSession.Id }, response);
        }

        // DELETE: api/GameSessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameSession(int id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);
            if (gameSession == null)
            {
                return NotFound();
            }

            _context.GameSessions.Remove(gameSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameSessionExists(int id)
        {
            return _context.GameSessions.Any(e => e.Id == id);
        }
    }
}
