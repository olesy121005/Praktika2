using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeatExchangeApp.Models;
using HeatExchangeApp.Data;
using HeatExchangeApp.Services;

namespace HeatExchangeApp.Controllers
{
    public class CalculationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHeatExchangeCalculator _calculator;

        public CalculationController(ApplicationDbContext context, IHeatExchangeCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        public async Task<IActionResult> Index()
        {
            var parameters = await _context.CalculationParameters
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
            return View(parameters);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CalculationParameters());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalculationParameters parameters)
        {
            if (ModelState.IsValid)
            {
                parameters.CreatedDate = DateTime.Now;
                _context.CalculationParameters.Add(parameters);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parameters);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var parameters = await _context.CalculationParameters.FindAsync(id);
            if (parameters == null)
            {
                return NotFound();
            }
            return View(parameters);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CalculationParameters parameters)
        {
            if (id != parameters.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    parameters.CreatedDate = DateTime.Now;
                    _context.Update(parameters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CalculationParametersExists(parameters.Id))
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
            return View(parameters);
        }

        public async Task<IActionResult> Calculate(int id)
        {
            var parameters = await _context.CalculationParameters.FindAsync(id);
            if (parameters == null)
            {
                return NotFound();
            }

            var results = _calculator.CalculateDetailed(parameters);
            ViewBag.Parameters = parameters;

            return View(results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var parameters = await _context.CalculationParameters.FindAsync(id);
            if (parameters != null)
            {
                _context.CalculationParameters.Remove(parameters);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CalculationParametersExists(int id)
        {
            return await _context.CalculationParameters.AnyAsync(e => e.Id == id);
        }
    }
}