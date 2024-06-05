using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock2u.Data;
using Stock2u.DTO;
using Stock2u.Models;

namespace Stock2u.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueRestaurantesController : ControllerBase
    {
        private readonly StokContext _context;
        private readonly IMapper _mapper;

        public EstoqueRestaurantesController(StokContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/EstoqueRestaurantes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EstoqueRestaurante>>> GetEstoqueRestaurantes()
        {
          if (_context.EstoqueRestaurantes == null)
          {
              return NotFound();
          }
            return await _context.EstoqueRestaurantes.ToListAsync();
        }

        // GET: api/EstoqueRestaurantes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EstoqueRestaurante>> GetEstoqueRestaurante(int id)
        {
          if (_context.EstoqueRestaurantes == null)
          {
              return NotFound();
          }
            var estoqueRestaurante = await _context.EstoqueRestaurantes.FindAsync(id);

            if (estoqueRestaurante == null)
            {
                return NotFound();
            }

            _context.Entry(estoqueRestaurante).Collection(x => x.Produtos).Load();

            return estoqueRestaurante;
        }

        // PUT: api/EstoqueRestaurantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEstoqueRestaurante(int id, EstoqueRestaurante estoqueRestaurante)
        {
            if (id != estoqueRestaurante.ID)
            {
                return BadRequest();
            }

            _context.Entry(estoqueRestaurante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstoqueRestauranteExists(id))
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

        // POST: api/EstoqueRestaurantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EstoqueRestaurante>> PostEstoqueRestaurante(EstoqueRestaurantePost estoqueRestauranteDTO)
        {
          if (_context.EstoqueRestaurantes == null)
          {
              return Problem("Entity set 'StokContext.EstoqueRestaurantes'  is null.");
          }
            var estoqueRestaurante = _mapper.Map<EstoqueRestaurante>(estoqueRestauranteDTO);
            _context.EstoqueRestaurantes.Add(estoqueRestaurante);
            await _context.SaveChangesAsync();
            _context.Entry(estoqueRestaurante).Collection(x => x.Produtos).Load();

            return CreatedAtAction("GetEstoqueRestaurante", new { id = estoqueRestaurante.ID }, estoqueRestaurante);
        }

        // DELETE: api/EstoqueRestaurantes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEstoqueRestaurante(int id)
        {
            if (_context.EstoqueRestaurantes == null)
            {
                return NotFound();
            }
            var estoqueRestaurante = await _context.EstoqueRestaurantes.FindAsync(id);
            if (estoqueRestaurante == null)
            {
                return NotFound();
            }

            _context.EstoqueRestaurantes.Remove(estoqueRestaurante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstoqueRestauranteExists(int id)
        {
            return (_context.EstoqueRestaurantes?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
