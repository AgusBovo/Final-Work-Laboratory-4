using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectofinalLabo4.Models;

namespace ProyectofinalLabo4.Controllers
{
    public class JugadoresController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IWebHostEnvironment env;

        public JugadoresController(AppDBcontext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Jugadores
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Jugadores.ToListAsync());
        }

        public async Task<IActionResult> Importar()
        {
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivo = archivos[0];

                if (archivo.Length > 0)
                {
                    var pathDestino = Path.Combine(env.WebRootPath, "Importaciones\\Jugadores");
                    var archivoDestino = Guid.NewGuid().ToString();
                    archivoDestino = archivoDestino.Replace("-", "");
                    archivoDestino += Path.GetExtension(archivo.FileName);
                    var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                    using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                    {
                        archivo.CopyTo(filestream);


                    };
                    using (var file = new FileStream(rutaDestino, FileMode.Create))
                    {
                        List<string> renglones = new List<string>();
                        List<Jugador> JugadorArchivo = new List<Jugador>();

                        StreamReader fileContent = new StreamReader(file, System.Text.Encoding.Default);
                        do
                        {
                            renglones.Add(fileContent.ReadLine());
                        }
                        while (fileContent.EndOfStream);
                        if(renglones.Count > 0)
                        {
                            foreach (var row in renglones)
                            {

                                string[] data = row.Split(";");
                                if (data.Length == 3)
                                {
                                    Jugador jugador = new Jugador();
                                    jugador.Nombre = data[0].Trim();
                                    jugador.Apellido = data[1].Trim();
                                    jugador.Biografia = data[2].Trim();

                                    JugadorArchivo.Add(jugador);
                                }

                            }
                            if (JugadorArchivo.Count > 0)
                            {
                                _context.AddRange(JugadorArchivo);
                                await _context.SaveChangesAsync();
                            }
                        }
                        
                        
                        
                    }



                }
            }
            
            return View("Index", await _context.Jugadores.ToListAsync());
        }

        // GET: Jugadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jugador == null)
            {
                return NotFound();
            }

            return View(jugador);
        }

        // GET: Jugadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jugadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Biografia")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count> 0)
                {
                    var archivoFoto = archivos[0];
                    
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "Imagenes\\Jugadores");
                        var archivoDestino = Guid.NewGuid().ToString();
                        archivoDestino = archivoDestino.Replace("-", "");
                        archivoDestino += Path.GetExtension(archivoFoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            jugador.foto = archivoDestino;
                        };
                    }
                }
                _context.Add(jugador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jugador);
        }

        // GET: Jugadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null)
            {
                return NotFound();
            }
            return View(jugador);
        }

        // POST: Jugadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Biografia,foto")] Jugador jugador)
        {
            if (id != jugador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];

                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "Imagenes\\Jugadores");
                        var archivoDestino = Guid.NewGuid().ToString();
                        archivoDestino = archivoDestino.Replace("-", "");
                        archivoDestino += Path.GetExtension(archivoFoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        string fotoAnterior = Path.Combine(pathDestino, jugador.foto);
                        if (System.IO.File.Exists(fotoAnterior))
                            System.IO.File.Delete(fotoAnterior);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            jugador.foto = archivoDestino;
                        };
                    }
                }
                try
                {
                    _context.Update(jugador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JugadorExists(jugador.Id))
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
            return View(jugador);
        }

        // GET: Jugadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jugador == null)
            {
                return NotFound();
            }

            return View(jugador);
        }

        // POST: Jugadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);
            _context.Jugadores.Remove(jugador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JugadorExists(int id)
        {
            return _context.Jugadores.Any(e => e.Id == id);
        }
    }
}
