using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectofinalLabo4.Models;

namespace ProyectofinalLabo4.Controllers
{
    public class JugadoresClubsController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IWebHostEnvironment env;

        public JugadoresClubsController(AppDBcontext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: JugadoresClubs
        public async Task<IActionResult> Index()
        {
            return View(await _context.JugadoresClubs.ToListAsync());
        }

        // GET: JugadoresClubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugadorClub = await _context.JugadoresClubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jugadorClub == null)
            {
                return NotFound();
            }

            return View(jugadorClub);
        }

        // GET: JugadoresClubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JugadoresClubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,idJugador,idClub")] JugadorClub jugadorClub)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jugadorClub);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jugadorClub);
        }

        // GET: JugadoresClubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugadorClub = await _context.JugadoresClubs.FindAsync(id);
            if (jugadorClub == null)
            {
                return NotFound();
            }
            return View(jugadorClub);
        }

        // POST: JugadoresClubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,idJugador,idClub")] JugadorClub jugadorClub)
        {
            if (id != jugadorClub.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jugadorClub);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JugadorClubExists(jugadorClub.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jugadorClub);
        }

        // GET: JugadoresClubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugadorClub = await _context.JugadoresClubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jugadorClub == null)
            {
                return NotFound();
            }

            return View(jugadorClub);
        }

        // POST: JugadoresClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jugadorClub = await _context.JugadoresClubs.FindAsync(id);
            _context.JugadoresClubs.Remove(jugadorClub);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JugadorClubExists(int id)
        {
            return _context.JugadoresClubs.Any(e => e.Id == id);
        }
    }
}
