using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransportSystemMVC.DbContext;
using TransportSystemMVC.Models;

namespace TransportSystemMVC.Controllers
{
    public class TrucksController : Controller
    {
        private readonly TransportDBContext _context;

        public TrucksController(TransportDBContext context)
        {
            _context = context;
        }

        // GET: Trucks
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var transportDBContext = _context.Trucks.Include(t => t.RouteFrom).Include(t => t.RouteTo).Include(t => t.Status);
            return View(await transportDBContext.ToListAsync());
        }

        // GET: Trucks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks
                .Include(t => t.RouteFrom)
                .Include(t => t.RouteTo)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TruckNro == id);
            if (truck == null)
            {
                return NotFound();
            }

            return View(truck);
        }

        // GET: Trucks/Create
        public IActionResult Create()
        {
            ViewData["RouteFromId"] = new SelectList(_context.Branches, "Id", "Id");
            ViewData["RouteToId"] = new SelectList(_context.Branches, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            return View();
        }

        // POST: Trucks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TruckNro,TruckModel,InsuranceName,Owner,Mobile,RouteFromId,StatusId,RouteToId")] Truck truck)
        {

                _context.Add(truck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }

        // GET: Trucks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }
            ViewData["RouteFromId"] = new SelectList(_context.Branches, "Id", "Id", truck.RouteFromId);
            ViewData["RouteToId"] = new SelectList(_context.Branches, "Id", "Id", truck.RouteToId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", truck.StatusId);
            return View(truck);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TruckNro,TruckModel,InsuranceName,Owner,Mobile,RouteFromId,StatusId,RouteToId")] Truck truck)
        {
            if (id != truck.TruckNro)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(truck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TruckExists(truck.TruckNro))
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

        // GET: Trucks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks
                .Include(t => t.RouteFrom)
                .Include(t => t.RouteTo)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TruckNro == id);
            if (truck == null)
            {
                return NotFound();
            }

            return View(truck);
        }

        // POST: Trucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TruckExists(int id)
        {
            return _context.Trucks.Any(e => e.TruckNro == id);
        }
    }
}
