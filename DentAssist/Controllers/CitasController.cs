using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;
using DentAssist.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DentAssist.Controllers
{
    [Authorize(Roles = "Recepcionista,Odontologo")]
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CitasController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Odontologo"))
            {
                var odontologo = await _context.Odontologos
                    .FirstOrDefaultAsync(o => o.UserId == userId);

                if (odontologo == null)
                {
                    return NotFound("No se encontró al odontólogo asociado al usuario.");
                }

                var citasOdontologo = await _context.Citas
                    .Include(c => c.Odontologo)
                    .Include(c => c.Paciente)
                    .Where(c => c.OdontologoId == odontologo.Id && c.Fecha >= DateTime.Now)
                    .OrderBy(c => c.Fecha)
                    .ToListAsync();

                return View(citasOdontologo);
            }

            // Si es otro rol, mostrar todas las citas
            var appDbContext = _context.Citas
                .Include(c => c.Odontologo)
                .Include(c => c.Paciente)
                .OrderBy(c => c.Fecha);

            return View(await appDbContext.ToListAsync());
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Odontologo)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            // Obtener tratamientos del paciente
            var tratamientos = await _context.Tratamientos
                .Where(t => t.PacienteId == cita.PacienteId && t.OdontologoId == cita.OdontologoId)
                .ToListAsync();

            ViewData["Tratamientos"] = tratamientos;

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            var model = new CitaViewModel
            {
                Fecha = DateTime.Now,
                DuracionMinutos = 30,
                Pacientes = _context.Pacientes
                .Include(p => p.User)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Nombre} {p.Apellido} ({p.RUT})"
                }).ToList(),

                Odontologos = _context.Odontologos
                .Include(o => o.User)
                .Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"{o.Nombre} {o.Apellido} - {o.Especialidad!.Nombre}"
                }).ToList()
            };
            return View(model);
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Pacientes = _context.Pacientes
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Nombre} {p.Apellido}"
                    }).ToList();

                model.Odontologos = _context.Odontologos
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = $"{o.Nombre} {o.Apellido}"
                    }).ToList();

                return View(model);
            }

            var cita = new Cita
            {
                Id = Guid.NewGuid(),
                Fecha = model.Fecha,
                Duracion = TimeSpan.FromMinutes(model.DuracionMinutos),
                PacienteId = model.PacienteId,
                OdontologoId = model.OdontologoId,
                Estado = EstadoCita.Pendiente,
                Observaciones = model.Observaciones
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["OdontologoId"] = new SelectList(
                _context.Odontologos
                    .Select(o => new {
                        o.Id,
                        NombreCompleto = $"{o.Nombre} {o.Apellido} - {o.Especialidad!.Nombre}"
                    }),
                "Id",
                "NombreCompleto",
                cita.OdontologoId
            );
            ViewData["PacienteId"] = new SelectList(
                _context.Pacientes
                    .Select(p => new {
                        p.Id,
                        NombreCompleto = $"{p.Nombre} {p.Apellido} ({p.RUT})"
                    }),
                "Id",
                "NombreCompleto",
                cita.PacienteId
            );
            ViewData["Estado"] = new SelectList(Enum.GetValues(typeof(EstadoCita)));
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Fecha,Duracion,PacienteId,OdontologoId,Estado,Observaciones")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Id", cita.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Id", cita.PacienteId);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Odontologo)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(Guid id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }
    }
}
