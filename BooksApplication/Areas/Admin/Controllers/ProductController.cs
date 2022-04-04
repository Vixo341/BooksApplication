using BooksApplication.DataAccess;
using BooksApplication.DataAccess.Repository.IRepository;
using BooksApplication.Models;
using BooksApplication.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksApplication.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment hostEnvironment)
        {
            _unitofWork = unitofWork;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitofWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryID.ToString()
                }),
                CoverTypeList = _unitofWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CoverTypeName,
                    Value = i.CoverTypeID.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitofWork.Product.GetFirstOrDefault(u => u.ProductId == id);
                return View(productVM);

                //update product
            }


        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (obj.Product.ProductId == 0)
                {
                    _unitofWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitofWork.Product.Update(obj.Product);
                }
                _unitofWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitofWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitofWork.Product.GetFirstOrDefault(u => u.ProductId == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitofWork.Product.Remove(obj);
            _unitofWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
