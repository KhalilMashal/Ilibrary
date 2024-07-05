

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ilibrary.DataAccess.Data;
using Ilibrary.Models;
using Ilibrary.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ilibrary.Models.ViewModels;
using Ilibrary.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace IlibraryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;
       
        public CompanyController(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
              
            return View(objCompanyList);
        }

   
        public IActionResult Upsert(int? id)
        {
            
            
            if(id ==  null || id== 0){
                //create
                return View(new Company() );
            }
            else
            {
                //update
                Company companyobj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyobj); 
            }
            
        }




        //[HttpPost]
        //public IActionResult Create(CompanyVM companyVM)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Add(companyVM.Company);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company Created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {

        //        companyVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });

        //        return View(companyVM);
        //    }
            
        //}

        [HttpPost]
        public IActionResult Upsert(Company companyobj)
        {


            if (ModelState.IsValid)
            {
               

                if (companyobj.Id== 0)
                {
                    _unitOfWork.Company.Add(companyobj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyobj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company Created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                

                return View(companyobj);
            }

        }



        //------------------------------Edit---------------------------------------------
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    /*way 1*/
        //    Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //way 2  //Company companyFromD1 = _db.categories.FirstOrDefault(u => u.Id==id);
        //    //way 3 //Company companyFromDb2 = _db.categories.Where(u => u.Id == id).FirstOrDefault();
        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(companyFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company Updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();


        //}
        //------------------------------Delete---------------------------------------------
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    /*way 1*/
        //    Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //way 2  //Company companyFromD1 = _db.categories.FirstOrDefault(u => u.Id==id);
        //    //way 3 //Company companyFromDb2 = _db.categories.Where(u => u.Id == id).FirstOrDefault();
        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(companyFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{

        //    Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company deleted successfully";
        //    return RedirectToAction("Index");
        //}
        //#region API CALLS
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<Company> objCompanyList = _unitOfWork.Company.GetAll(  ).ToList();
        //    return Json(new { data = objCompanyList });


        //}

        //#endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }
} 
