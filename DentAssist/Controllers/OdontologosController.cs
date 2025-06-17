using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models.Data;
using DentAssist.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DentAssist.Models.ViewModels;
using System.Globalization;

namespace DentAssist.Controllers
{
    public class OdontologosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OdontologosController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Odontologos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Odontologos.Include(o => o.Especialidad).Include(o => o.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Odontologos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var odontologo = await _context.Odontologos
                .Include(o => o.Especialidad)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (odontologo == null)
                return NotFound();

            var today = DateTime.Today;
            int delta = DayOfWeek.Monday - today.DayOfWeek;
            var startOfWeek = today.AddDays(delta);
            var endOfWeek = startOfWeek.AddDays(7);

            // Obtener citas del odontologo
            var citas = await _context.Citas
                .Where(c => c.OdontologoId == odontologo.Id && c.Fecha >= startOfWeek && c.Fecha < endOfWeek)
                .Include(c => c.Paciente)
                .OrderBy(c => c.Fecha)
                .ToListAsync();

            ViewData["CitasSemana"] = citas;

            return View(odontologo);
        }

        // GET: Odontologos/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "Id", "Nombre");
            return View();
        }

        // POST: Odontologos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(OdontologoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "Id", "Nombre", model.EspecialidadId);
                return View(model);
            }

            // Crear cuenta de usuario
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "Id", "Nombre", model.EspecialidadId);
                return View(model);
            }

            // Agregar al rol de Odontólogo (si usás roles)
            await _userManager.AddToRoleAsync(user, "Odontologo");

            // Crear entidad Odontologo
            var odontologo = new Odontologo
            {
                Id = Guid.NewGuid(),
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Matricula = model.Matricula,
                EspecialidadId = model.EspecialidadId,
                UserId = user.Id
            };

            _context.Odontologos.Add(odontologo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Odontologos/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odontologo = await _context.Odontologos.FindAsync(id);
            if (odontologo == null)
            {
                return NotFound();
            }
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "Id", "Nombre", odontologo.EspecialidadId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", odontologo.UserId);
            return View(odontologo);
        }

        // POST: Odontologos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Apellido,Matricula,EspecialidadId,UserId")] Odontologo odontologo)
        {
            if (id != odontologo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(odontologo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdontologoExists(odontologo.Id))
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
            ViewData["EspecialidadId"] = new SelectList(_context.Especialidades, "Id", "Id", odontologo.EspecialidadId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", odontologo.UserId);
            return View(odontologo);
        }

        // GET: Odontologos/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odontologo = await _context.Odontologos
                .Include(o => o.Especialidad)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odontologo == null)
            {
                return NotFound();
            }

            return View(odontologo);
        }

        // POST: Odontologos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var odontologo = await _context.Odontologos.FindAsync(id);
            if (odontologo != null)
            {
                _context.Odontologos.Remove(odontologo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdontologoExists(Guid id)
        {
            return _context.Odontologos.Any(e => e.Id == id);
        }
    }
}
