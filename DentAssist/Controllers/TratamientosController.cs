using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace DentAssist.Controllers
{
    [Authorize(Roles = "Odontologo")]
    public class TratamientosController : Controller
    {
        private readonly AppDbContext _context;

        public TratamientosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tratamientos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Tratamientos.Include(t => t.Odontologo).Include(t => t.Paciente);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Tratamientos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Nombre");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre");
            return View();
        }

        // POST: Tratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Costo,CantidadSesiones,PacienteId,OdontologoId")] Tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                tratamiento.Id = Guid.NewGuid();
                _context.Add(tratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Nombre", tratamiento.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", tratamiento.PacienteId);
            return View(tratamiento);
        }

        // GET: Tratamientos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento == null)
            {
                return NotFound();
            }
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Nombre", tratamiento.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", tratamiento.PacienteId);
            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Descripcion,Costo,CantidadSesiones,PacienteId,OdontologoId")] Tratamiento tratamiento)
        {
            if (id != tratamiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TratamientoExists(tratamiento.Id))
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
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Nombre", tratamiento.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", tratamiento.PacienteId);
            return View(tratamiento);
        }

        // GET: Tratamientos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // POST: Tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento != null)
            {
                _context.Tratamientos.Remove(tratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TratamientoExists(Guid id)
        {
            return _context.Tratamientos.Any(e => e.Id == id);
        }
    }
}
