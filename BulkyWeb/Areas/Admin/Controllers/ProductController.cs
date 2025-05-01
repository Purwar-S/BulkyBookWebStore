using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BulkyBook.Models.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll().ToList();
            return View(productList);
        }
        public IActionResult Create()
        {
            //projections in EF core (picking only some clos not all directly)
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            //using ViewData to pass data from controller to view (not vice-versa)
            //ViewData needs explicit casting in views
            //ViewData["CategoriesList"] = categoryList;
            ProductVM product_VM_Obj = new() 
            {
                Product = new Product(),
                CategoryList = categoryList
            };
            return View(product_VM_Obj);
        }
        [HttpPost]
        public IActionResult Create(Product model)
        {
            // --- custom validations
            //if (model.Name == model.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("", "Name and Display Order annnot be same");
            //}

            // --- server side validation
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(model);
                _unitOfWork.Save();
                TempData["success"] = "Product Added Successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Product editProduct = _unitOfWork.Product.GetValue(u => u.Id == Id);
            //Category editCategory1 = _db.Categories.FirstOrDefault(u => u.Id == Id);
            //Category editCategory1 = _db.Categories.Where(u => u.Id == Id).FirstOrDefault();

            if (editProduct == null)
            {
                return NotFound();
            }
            //projections in EF core (picking only some clos not all directly)
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            //using ViewBag -method to transfer data from controller to view
            //ViewBag do not need explicit casting unlike ViewData
            ViewBag.CategoriesList = categoryList;
            return View(editProduct);
        }
        [HttpPost]
        public IActionResult Edit(Product item)
        {
            _unitOfWork.Product.Update(item);
            _unitOfWork.Save();
            TempData["success"] = "Product Updated Successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Product deleteItem = _unitOfWork.Product.GetValue(u => u.Id == Id);

            if (deleteItem == null)
            {
                return NotFound();
            }
            return View(deleteItem);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            Product item = _unitOfWork.Product.GetValue(u => u.Id == Id);
            if (item == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(item);
            _unitOfWork.Save();
            TempData["success"] = "Product Item Deleted Successfully";
            return RedirectToAction("Index");
        }
    }

}
