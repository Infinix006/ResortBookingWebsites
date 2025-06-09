using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new ()
            {
                VillaList = _unitOfWork.Villa.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            //ViewData["VillaList"] = list;       //   

            //ViewBag.VillaList = list;

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //ModelState.Remove("Villa");
            if (!ModelState.IsValid)
            {
                TempData["error"] = "The Villa Number could not be created.";
                return View(obj);
            }

            var villa = _unitOfWork.VillaNumber.Any(e => e.Villa_Number == obj.VillaNumber.Villa_Number);
            if(villa)
            {
                TempData["error"] = "The Villa Number already exists..";

                obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(obj);
            }
            _unitOfWork.VillaNumber.Add(obj.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "The Villa Number has been created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int VillaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(i => i.Villa_Number == VillaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error","Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (!ModelState.IsValid || villaNumberVM.VillaNumber.Villa_Number == 0)
            {
                villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                
                TempData["error"] = "The Villa Number could not be updated.";
                return View(villaNumberVM);
            }

            _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "The Villa Number has been updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int VillaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(i => i.Villa_Number == VillaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(i => i.Villa_Number == obj.VillaNumber.Villa_Number);

            if(objFromDb is null)
            {
                TempData["error"] = "The Villa Number could not be deleted.";
                return RedirectToAction("Error", "Home");
            }

            _unitOfWork.VillaNumber.Remove(objFromDb);
            _unitOfWork.Save();

            TempData["success"] = "The Villa Number has been deleted successfully.";
         return RedirectToAction(nameof(Index));
        }
    }
}
