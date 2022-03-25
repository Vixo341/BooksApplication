using BooksApplication.DataAccess;
using BooksApplication.DataAccess.Repository.IRepository;
using BooksApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksApplication.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CoverTypeController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }


        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitofWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Add(obj);
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _unitofWork.CoverType.GetFirstOrDefault(u=>u.CoverTypeID == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
           
            return View(coverTypeFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Update(obj);
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _unitofWork.CoverType.GetFirstOrDefault(builder=>builder.CoverTypeID == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? coverTypeID)
        {
            var coverTypeFromDb = _unitofWork.CoverType.GetFirstOrDefault(builder => builder.CoverTypeID == coverTypeID); 
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            _unitofWork.CoverType.Remove(coverTypeFromDb);
            _unitofWork.Save();
                return RedirectToAction("Index");
            
        }
    }
}
