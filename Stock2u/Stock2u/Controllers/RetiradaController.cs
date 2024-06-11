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
    public class RetiradaController : ControllerBase
    {
        private readonly StokContext _context;
        private readonly IMapper _mapper;

        public RetiradaController(StokContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Retirada
        [HttpGet]
        [Authorize(Roles = "Admin,client")]
        public async Task<ActionResult<IEnumerable<RetiradaGet>>> GetRetiradas()
        {
          if (_context.Retiradas == null)
          {
              return NotFound();
          }
          var retiradas = await _context.Retiradas.Include(r => r.Produto).ToListAsync();
          var retiradasGet = _mapper.Map<List<RetiradaGet>>(retiradas);
          return retiradasGet;
        }

        // GET: api/Retirada/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,client")]
        public async Task<ActionResult<Retirada>> GetRetirada(Guid id)
        {
          if (_context.Retiradas == null)
          {
              return NotFound();
          }
            var retirada = await _context.Retiradas.FindAsync(id);

            if (retirada == null)
            {
                return NotFound();
            }
            _context.Entry(retirada).Reference(x => x.Produto).Load();
            return retirada;
        }

        // POST: api/Retirada
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,client")]
        public async Task<ActionResult<Retirada>> PostRetirada(RetiradaPost retiradaPost)
        {
          if (_context.Retiradas == null)
          {
              return Problem("Entity set 'StokContext.Retiradas'  is null.");
          }

            var retirada = _mapper.Map<Retirada>(retiradaPost);

            var produto = await _context.Produtos.FindAsync(retirada.IdProduto);

            produto.Quantity -= retirada.Quantity;
            
            if (produto.Quantity < 0)
            {
                return Problem("the quantity may be positive or zero");
            }
            else if (produto.Quantity == 0)
            {
                produto.Avaliable = false;
            }
            _context.Entry(produto).State = EntityState.Modified;
            
            _context.Retiradas.Add(retirada);
            await _context.SaveChangesAsync();
            _context.Entry(retirada).Reference(x => x.Produto).Load();
            return CreatedAtAction("GetRetirada", new { id = retirada.ID }, retirada);
        }

        private bool RetiradaExists(Guid id)
        {
            return (_context.Retiradas?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
