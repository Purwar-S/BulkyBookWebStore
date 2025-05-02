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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(productList);
        }
        public IActionResult Upsert(int? id)
        {
            //projections in EF core (picking only some clos not all directly)
            ProductVM product_VM_Obj = new()
            {
                CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            //insert or create
            if (id ==0 || id == null) //insert
            {
                
                return View(product_VM_Obj);
            }
            else
            //update
            {
                product_VM_Obj.Product = _unitOfWork.Product.GetValue(u => u.Id == id);
                return View(product_VM_Obj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
                        
            if (ModelState.IsValid)
            {
                string path = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productImgPath = Path.Combine(path, @"images\products");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //Delete the old image and then add new one 
                        string oldImgPath = Path.Combine(path, productVM.Product.ImageUrl.Trim('\\'));
                        if(System.IO.File.Exists(oldImgPath)) 
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    //saving to product image to file path above
                    using(var filestream = new FileStream(Path.Combine(productImgPath,filename),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + filename;
                }

                if(productVM.Product.Id != 0)//no Id for object means Create new one
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Updated Successfully";
                }
                else
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Added Successfully";
                }
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                //ProductVM viewmodel = new()
                //{
                //    CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem{
                //                                                                                    Text = u.Name,
                //                                                                                    Value = u.Id.ToString()}),
                //    Product = new Product()
                //};
                return View(productVM);
            }
        }
        //public IActionResult Edit(int? Id)
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product editProduct = _unitOfWork.Product.GetValue(u => u.Id == Id);
        //    //Category editCategory1 = _db.Categories.FirstOrDefault(u => u.Id == Id);
        //    //Category editCategory1 = _db.Categories.Where(u => u.Id == Id).FirstOrDefault();

        //    if (editProduct == null)
        //    {
        //        return NotFound();
        //    }
        //    //projections in EF core (picking only some clos not all directly)
        //    IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
        //        .Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });
        //    //using ViewBag -method to transfer data from controller to view
        //    //ViewBag do not need explicit casting unlike ViewData
        //    ViewBag.CategoriesList = categoryList;
        //    return View(editProduct);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product item)
        //{
        //    _unitOfWork.Product.Update(item);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Updated Successfully";
        //    return RedirectToAction("Index");
        //}

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
