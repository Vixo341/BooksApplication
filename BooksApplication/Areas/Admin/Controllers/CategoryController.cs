using BooksApplication.DataAccess;
using BooksApplication.DataAccess.Repository.IRepository;
using BooksApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksApplication.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }


        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitofWork.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.CategoryName == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CategoryName", "The displayorder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Add(obj);
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
            var categoryFromDb = _unitofWork.Category.GetFirstOrDefault(u=>u.CategoryID == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
           
            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.CategoryName == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CategoryName", "The displayorder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Update(obj);
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
            var categoryFromDb = _unitofWork.Category.GetFirstOrDefault(builder=>builder.CategoryID == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? categoryID)
        {
            var categoryFromDb = _unitofWork.Category.GetFirstOrDefault(builder => builder.CategoryID == categoryID); 
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            _unitofWork.Category.Remove(categoryFromDb);
            _unitofWork.Save();
                return RedirectToAction("Index");
            
        }
    }
}
