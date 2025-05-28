using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var villas = _context.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "The villa could not be created.";
                return View(obj);
            }
            _context.Villas.Add(obj);
            _context.SaveChanges();
            TempData["success"] = "The villa has been created successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _context.Villas.FirstOrDefault(i => i.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error","Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (!ModelState.IsValid || obj.Id == 0)
            {
                TempData["error"] = "The villa could not be updated.";
                return View(obj);
            }

            _context.Villas.Update(obj);
            _context.SaveChanges();
            TempData["success"] = "The villa has been updated successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _context.Villas.FirstOrDefault(i => i.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _context.Villas.FirstOrDefault(i => i.Id == obj.Id);

            if(objFromDb is null)
            {
                TempData["error"] = "The villa could not be deleted.";
                return RedirectToAction("Error", "Home");
            }

            _context.Villas.Remove(objFromDb);
            _context.SaveChanges();

            TempData["success"] = "The villa has been deleted successfully.";
         return RedirectToAction("Index");
        }
    }
}
