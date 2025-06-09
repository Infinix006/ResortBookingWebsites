using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
            _unitOfWork.Villa.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "The villa has been created successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
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

            _unitOfWork.Villa.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "The villa has been updated successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(i => i.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(i => i.Id == obj.Id);

            if (objFromDb is null)
            {
                TempData["error"] = "The villa could not be deleted.";
                return RedirectToAction("Error", "Home");
            }

            _unitOfWork.Villa.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["success"] = "The villa has been deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
