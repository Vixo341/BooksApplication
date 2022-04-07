using BooksApplication.DataAccess;
using BooksApplication.DataAccess.Repository.IRepository;
using BooksApplication.Models;
using BooksApplication.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksApplication.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitofWork _unitofWork;


        public CompanyController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new();


            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitofWork.Company.GetFirstOrDefault(u => u.CompanyId == id);
                return View(company);

            }


        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if (obj.CompanyId == 0)
                {
                    _unitofWork.Company.Add(obj);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _unitofWork.Company.Update(obj);
                    TempData["success"] = "Company updated successfully";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var pcompanyList = _unitofWork.Company.GetAll();
            return Json(new { data = pcompanyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitofWork.Company.GetFirstOrDefault(u => u.CompanyId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitofWork.Company.Remove(obj);
            _unitofWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
