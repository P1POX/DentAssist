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
using DentAssist.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DentAssist.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RecepcionistasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecepcionistasController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Recepcionistas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Recepcionistas.Include(r => r.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Recepcionistas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionistas
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // GET: Recepcionistas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recepcionistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecepcionistaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Crear usuario
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // Asignar rol al usuario
            await _userManager.AddToRoleAsync(user, "Recepcionista");

            // Crear recepcionista y guardar en DB
            var recepcionista = new Recepcionista
            {
                Id = Guid.NewGuid(),
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                UserId = user.Id
            };

            _context.Recepcionistas.Add(recepcionista);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Recepcionistas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionistas.FindAsync(id);
            if (recepcionista == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", recepcionista.UserId);
            return View(recepcionista);
        }

        // POST: Recepcionistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Apellido,UserId")] Recepcionista recepcionista)
        {
            if (id != recepcionista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recepcionista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionistaExists(recepcionista.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recepcionista.UserId);
            return View(recepcionista);
        }

        // GET: Recepcionistas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionistas
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // POST: Recepcionistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recepcionista = await _context.Recepcionistas.FindAsync(id);
            if (recepcionista != null)
            {
                _context.Recepcionistas.Remove(recepcionista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecepcionistaExists(Guid id)
        {
            return _context.Recepcionistas.Any(e => e.Id == id);
        }
    }
}
