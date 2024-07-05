using Ilibrary.DataAccess.Data;
using Ilibrary.DataAccess.Repository.IRepository;
using Ilibrary.Models;
using Ilibrary.Models.ViewModels;
using Ilibrary.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace IlibraryWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork  _unitOfWork;
        private readonly ApplicationDbContext _context;



        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork , ApplicationDbContext context )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _context = context;


        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");

            var query = from product in _context.Products
                        join orderDetail in _context.OrderDetails
                        on product.Id equals orderDetail.ProductId into productOrders
                        orderby productOrders.Count() descending
                        select product;

            // Execute the query and convert the result to a list of products
            IEnumerable<Product> productsOrderedByOrderCount = query.ToList();

            return View(productsOrderedByOrderCount);
        }

        public IActionResult Filter(int id)
        {
            // Retrieve all products with the specified category ID
            var productList = _context.Products
                .Where(p => p.CategoryId == id)
                .ToList();

            return View(productList);
        }
        public IActionResult Details(int productId)
        {

            IEnumerable<Comment> allComments = _context.Comments.Where(c => c.ProductId == productId);
            ShoppingCart cart = new()

            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),

                Count = 1,
                ProductId = productId
            };
            //Product produc = _unitOfWork.Product.Get(u =>u.Id== productId, includeProperties: "Category");
            //return View(produc);
            var com = new Comment { ProductId = productId };
            DetailsVM detailsVM = new()
            {
                shoppingcarts = cart,
                comments = com,
                allcomments = allComments

            };
            return View(detailsVM);


        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(DetailsVM vm)

        {

            ShoppingCart shoppingCart = vm.shoppingcarts;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
            var indexOfAtSymbol = userName.IndexOf('@');
            if (indexOfAtSymbol != -1)
            {
                userName = userName.Substring(0, indexOfAtSymbol);

            }
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                //add cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";
            _unitOfWork.Save();



            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(DetailsVM model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userName = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
            var indexOfAtSymbol = userName.IndexOf('@');
            if (indexOfAtSymbol != -1)
            {
                userName = userName.Substring(0, indexOfAtSymbol);

            }

            Comment comm = model.comments;
            comm.ProductId = model.shoppingcarts.ProductId;
            comm.Rating = model.comments.Rating;
            comm.UserName = userName;
            int id;
            _context.Comments.Add(comm);
            _context.SaveChanges();
            //return RedirectToAction("Index", "Product", new { id = comm.ProductId });
            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id)
        {
            Comment comment =  _context.Comments.FirstOrDefault(i => i.Id == id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
    }
}
