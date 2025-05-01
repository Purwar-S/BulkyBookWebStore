using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll().ToList();

            return View(categoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
            //custom validations
            if (model.Name == model.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "Name and Display Order annnot be same");
            }
            //server side validation
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(model);
                _unitOfWork.Save();
                TempData["success"] = "Category Added Successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category editCategory = _unitOfWork.Category.GetValue(u => u.Id == Id);
            //Category editCategory1 = _db.Categories.FirstOrDefault(u => u.Id == Id);
            //Category editCategory1 = _db.Categories.Where(u => u.Id == Id).FirstOrDefault();

            if (editCategory == null)
            {
                return NotFound();
            }
            return View(editCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Updated Successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category deleteCategory = _unitOfWork.Category.GetValue(u => u.Id == Id);

            if (deleteCategory == null)
            {
                return NotFound();
            }
            return View(deleteCategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            Category category = _unitOfWork.Category.GetValue(u => u.Id == Id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }

}
